using Amazon.RDS;
using Amazon.RDS.Model;
using AWSSDK.Common;
using AWSSDK.Constants;
using AWSSDK.Interfaces;
using Domain.SuperAdminModels.Notification;
using Domain.SuperAdminModels.Tenant;
using Interactor.Realtime;
using UseCase.SuperAdmin.RestoreTenant;

namespace Interactor.SuperAdmin
{
    public class RestoreTenantInteractor : IRestoreTenantInputPort
    {
        private readonly IAwsSdkService _awsSdkService;
        private readonly ITenantRepository _tenantRepository;
        private readonly IWebSocketService _webSocketService;
        private readonly INotificationRepository _notificationRepository;
        public RestoreTenantInteractor(ITenantRepository tenantRepository, IAwsSdkService awsSdkService, IWebSocketService webSocketService, INotificationRepository notificationRepository)
        {
            _awsSdkService = awsSdkService;
            _tenantRepository = tenantRepository;
            _webSocketService = webSocketService;
            _notificationRepository = notificationRepository;
        }

        public RestoreTenantOutputData Handle(RestoreTenantInputData inputData)
        {
            //string rString2 = CommonConstants.GenerateRandomString(6);
            //var snapshotIdentifier = _awsSdkService.CreateDBSnapshotAsync("develop-smartkarte-postgres-nnefqy", ConfigConstant.RdsSnapshotBackupRestore);
            try
            {
                if (inputData.TenantId <= 0)
                {
                    return new RestoreTenantOutputData(false, RestoreTenantStatus.InvalidTenantId);
                }

                var tenant = _tenantRepository.Get(inputData.TenantId);
                if (tenant == null)
                {
                    return new RestoreTenantOutputData(false, RestoreTenantStatus.TenantDoesNotExist);
                }

                // Get laster snapshot restore to tmp tenant 
                var lastSnapshotIdentifier = RDSAction.GetLastSnapshot(tenant.RdsIdentifier).Result;
                if (string.IsNullOrEmpty(lastSnapshotIdentifier))
                {
                    return new RestoreTenantOutputData(false, RestoreTenantStatus.SnapshotNotAvailable);
                }

                CancellationTokenSource cts = new CancellationTokenSource();
                _ = Task.Run(async () =>
                {
                    try
                    {
                        _tenantRepository.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["restoring"]);
                        Console.WriteLine($"Start  restore  tenant. RdsIdentifier: {tenant.RdsIdentifier}");



                        Console.WriteLine($"Start Restore: {tenant.RdsIdentifier}");

                        // Create tmp RDS from snapshot
                        string rString = CommonConstants.GenerateRandomString(6);
                        var dbInstanceIdentifier = $"{tenant.SubDomain}-{rString}";
                        var isSuccessRestoreInstance = await _awsSdkService.RestoreDBInstanceFromSnapshot(dbInstanceIdentifier, lastSnapshotIdentifier);
                        var endpoint = await CheckRestoredInstanceAvailableAsync(dbInstanceIdentifier, inputData.TenantId);

                        // Restore tenant dedicate
                        if (tenant.Type == ConfigConstant.TypeDedicate)
                        {
                            // Create snapshot backup
                            var snapshotIdentifier = await _awsSdkService.CreateDBSnapshotAsync(tenant.RdsIdentifier, ConfigConstant.RdsSnapshotBackupRestore);

                            if (string.IsNullOrEmpty(snapshotIdentifier) || !await RDSAction.CheckSnapshotAvailableAsync(snapshotIdentifier))
                            {
                                throw new Exception("Snapshot is not Available");
                            }

                            // Update data enpoint

                            // Finished restore
                            _tenantRepository.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["available"]);
                            var messenge = $"{tenant.EndSubDomain} is restore successfully.";
                            var notification = _notificationRepository.CreateNotification(ConfigConstant.StatusNotiSuccess, messenge);
                            await _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, notification);
                            cts.Cancel();
                            return;
                        }

                        // Restore tenant sharing
                        else
                        {
                            // dump data, delete old db
                            _tenantRepository.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["available"]);
                            var messenge = $"{tenant.EndSubDomain} is restore successfully.";
                            var notification = _notificationRepository.CreateNotification(ConfigConstant.StatusNotiSuccess, messenge);
                            await _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, notification);
                            cts.Cancel();
                            return;
                        }

                        // Check Restore success 
                    }
                    catch (Exception ex)
                    {
                        _tenantRepository.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["restore-failed"]);
                        // Notification  restore failed
                        var messenge = $"{tenant.EndSubDomain} is restore failed. Error: {ex.Message}.";
                        var notification = _notificationRepository.CreateNotification(ConfigConstant.StatusNotifailure, messenge);
                        await _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, notification);
                        cts.Cancel();
                        return;
                    }
                });
                return new RestoreTenantOutputData(true, RestoreTenantStatus.Successed);
            }
            finally
            {
                _tenantRepository.ReleaseResource();
                _notificationRepository.ReleaseResource();
            }
        }

        public async Task<Endpoint> CheckRestoredInstanceAvailableAsync(string dbInstanceIdentifier, int tenantId)
        {
            try
            {
                DateTime startTime = DateTime.Now;
                bool running = true;
                string status = string.Empty;
                while (running)
                {
                    var rdsClient = new AmazonRDSClient();

                    // Create a request to describe DB instances
                    var describeInstancesRequest = new DescribeDBInstancesRequest
                    {
                        DBInstanceIdentifier = dbInstanceIdentifier
                    };

                    // Call DescribeDBInstancesAsync to asynchronously get information about the DB instance
                    var describeInstancesResponse = await rdsClient.DescribeDBInstancesAsync(describeInstancesRequest);

                    // Check if the DB instance exists
                    var dbInstances = describeInstancesResponse.DBInstances;
                    if (dbInstances.Count == 1)
                    {
                        var dbInstance = dbInstances[0];
                        var checkStatus = dbInstance.DBInstanceStatus;

                        if (status != checkStatus)
                        {
                            status = checkStatus;
                            var rdsStatusDictionary = ConfigConstant.StatusTenantDictionary();
                            if (rdsStatusDictionary.TryGetValue(checkStatus, out byte statusTenant))
                            {
                                _tenantRepository.UpdateStatusTenant(tenantId, statusTenant);
                            }
                        }
                        Console.WriteLine($"DB Instance status: {checkStatus}");

                        // Check if the DB instance is in the "available" state
                        if (status.Equals("available", StringComparison.OrdinalIgnoreCase))
                        {
                            running = false;
                            return describeInstancesResponse.DBInstances[0].Endpoint;
                        }
                    }
                    else
                    {
                        running = false;
                        throw new Exception($"Checking Restored Instance Available. DB instance doesn't exists");
                    }

                    // Check if more than timeout
                    if ((DateTime.Now - startTime).TotalMinutes > ConfigConstant.TimeoutCheckingAvailable)
                    {
                        Console.WriteLine($"Timeout: DB instance not available after {ConfigConstant.TimeoutCheckingAvailable} minutes.");
                        running = false;
                        throw new Exception($"Checking Restored Instance Available. Timeout");
                    }

                    // Wait for 5 seconds before the next attempt
                    Thread.Sleep(5000);
                }

                // Return an empty Endpoint if the loop exits without finding an available instance
                return new Endpoint();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception($"Checking Restored Instance Available. {ex.Message}");
            }
        }
    }
}
