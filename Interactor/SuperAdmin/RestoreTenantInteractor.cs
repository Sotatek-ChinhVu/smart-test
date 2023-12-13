using Amazon.RDS;
using Amazon.RDS.Model;
using AWSSDK.Common;
using AWSSDK.Constants;
using AWSSDK.Dto;
using AWSSDK.Interfaces;
using Domain.SuperAdminModels.Notification;
using Domain.SuperAdminModels.Tenant;
using Interactor.Realtime;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using UseCase.SuperAdmin.RestoreTenant;

namespace Interactor.SuperAdmin
{
    public class RestoreTenantInteractor : IRestoreTenantInputPort
    {
        private readonly IAwsSdkService _awsSdkService;
        private readonly ITenantRepository _tenantRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly ITenantRepository _tenantRepositoryRunTask;
        private readonly INotificationRepository _notificationRepositoryRunTask;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;
        public RestoreTenantInteractor(
            ITenantRepository tenantRepository,
            IAwsSdkService awsSdkService,
            INotificationRepository notificationRepository,
            ITenantRepository tenantRepositoryRunTask,
            INotificationRepository notificationRepositoryRunTask,
            IConfiguration configuration,
            IMemoryCache memoryCache
            )
        {
            _awsSdkService = awsSdkService;
            _tenantRepository = tenantRepository;
            _notificationRepository = notificationRepository;
            _tenantRepositoryRunTask = tenantRepositoryRunTask;
            _notificationRepositoryRunTask = notificationRepositoryRunTask;
            _configuration = configuration;
            _memoryCache = memoryCache;

        }

        public RestoreTenantOutputData Handle(RestoreTenantInputData inputData)
        {
            IWebSocketService _webSocketService;
            _webSocketService = (IWebSocketService)inputData.WebSocketService;
            string pathFileDumpRestore = _configuration["PathFileDumpRestore"] ?? string.Empty;

            if (string.IsNullOrEmpty(pathFileDumpRestore))
            {
                return new RestoreTenantOutputData(false, RestoreTenantStatus.PathFileDumpRestoreNotAvailable);
            }
            try
            {
                if (inputData.TenantId <= 0)
                {
                    return new RestoreTenantOutputData(false, RestoreTenantStatus.InvalidTenantId);
                }

                var tenant = _tenantRepository.Get(inputData.TenantId);
                if (tenant == null || tenant.TenantId <= 0)
                {
                    return new RestoreTenantOutputData(false, RestoreTenantStatus.TenantDoesNotExist);
                }

                // Get laster snapshot restore to tmp tenant 
                var lastSnapshotIdentifier = RDSAction.GetLastSnapshot(tenant.RdsIdentifier).Result;
                if (string.IsNullOrEmpty(lastSnapshotIdentifier))
                {
                    return new RestoreTenantOutputData(false, RestoreTenantStatus.SnapshotNotAvailable);
                }

                var cts = new CancellationTokenSource();
                CancellationToken ct = cts.Token;
                _ = Task.Run(() =>
                {
                    try
                    {
                        ct.ThrowIfCancellationRequested();
                        _tenantRepository.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["restoring"]);

                        // Set tenant info to cache memory
                        _memoryCache.Set(tenant.SubDomain, cts);

                        Console.WriteLine($"Start  restore  tenant. RdsIdentifier: {tenant.RdsIdentifier}");

                        // Create snapshot backup
                        var snapshotIdentifier = _awsSdkService.CreateDBSnapshotAsync(tenant.RdsIdentifier, ConfigConstant.RdsSnapshotBackupRestore).Result;

                        if (string.IsNullOrEmpty(snapshotIdentifier) || !RDSAction.CheckSnapshotAvailableAsync(snapshotIdentifier).Result)
                        {
                            throw new Exception("Snapshot is not Available");
                        }

                        // Create tmp RDS from snapshot
                        string rString = CommonConstants.GenerateRandomString(6);
                        var dbInstanceIdentifier = $"{tenant.SubDomain}-{rString}";

                        if (!ct.IsCancellationRequested) // Check task run is not canceled
                        {
                            var isSuccessRestoreInstance = _awsSdkService.RestoreDBInstanceFromSnapshot(dbInstanceIdentifier, lastSnapshotIdentifier).Result;
                            // Set tenant info to cache memory
                            _memoryCache.Set(tenant.SubDomain, new TenantCacheMemory(cts, dbInstanceIdentifier));
                        }

                        var endpoint = CheckRestoredInstanceAvailableAsync(dbInstanceIdentifier, inputData.TenantId).Result;

                        // Restore tenant dedicate
                        if (tenant.Type == ConfigConstant.TypeDedicate)
                        {
                            // Update data enpoint
                            bool updateEndPoint = false;
                            if (!ct.IsCancellationRequested) // Check task run is not canceled
                            {
                                updateEndPoint = _tenantRepositoryRunTask.UpdateInfTenant(tenant.TenantId, ConfigConstant.StatusTenantDictionary()["available"], tenant.EndSubDomain, endpoint.Address, dbInstanceIdentifier);
                            }

                            if (!updateEndPoint)
                            {
                                throw new Exception("Update end sub domain failed");
                            }

                            // delete old RDS
                            if (!ct.IsCancellationRequested) // Check task run is not canceled
                            {

                                var actionDeleteOldRDS = RDSAction.DeleteRDSInstanceAsync(tenant.RdsIdentifier).Result;
                                var checkDeleteActionOldRDS = RDSAction.CheckRDSInstanceDeleted(tenant.RdsIdentifier).Result;
                            }

                            // Finished restore
                            if (!ct.IsCancellationRequested) // Check task run is not canceled
                            {
                                _tenantRepositoryRunTask.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["available"]);
                                var messenge = $"{tenant.EndSubDomain} is restore successfully.";
                                var notification = _notificationRepositoryRunTask.CreateNotification(ConfigConstant.StatusNotiSuccess, messenge);
                                // Add info tenant for notification
                                notification.SetTenantId(tenant.TenantId);
                                notification.SetStatusTenant(tenant.StatusTenant);
                                _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, notification);
                            }
                            cts.Cancel();
                            return;
                        }

                        // Restore tenant sharing
                        else
                        {
                            // dump data,
                            var pathFileDump = @$"{pathFileDumpRestore}{tenant.Db}.sql"; // path save file sql dump
                            PostgresSqlAction.PostgreSqlDump(pathFileDump, endpoint.Address, ConfigConstant.PgPostDefault, tenant.Db, tenant.UserConnect, tenant.PasswordConnect).Wait();

                            // check valid file sql dump
                            if (!System.IO.File.Exists(pathFileDump))
                            {
                                throw new Exception("File sql dump doesn't exist");
                            }

                            if (!PostgresSqlAction.CheckingFinishedAccessedFile(pathFileDump))
                            {
                                throw new Exception("Invalid file sql dump");
                            }

                            // restore db 
                            PostgresSqlAction.PostgreSqlExcuteFileDump(pathFileDump, tenant.EndPointDb, ConfigConstant.PgPostDefault, tenant.Db, tenant.UserConnect, tenant.PasswordConnect).Wait();

                            if (!ct.IsCancellationRequested) // Check task run is not canceled
                            {
                                // delete Tmp RDS
                                var actionDeleteTmpRDS = RDSAction.DeleteRDSInstanceAsync(dbInstanceIdentifier, true).Result;
                                var checkDeleteActionTmpRDS = RDSAction.CheckRDSInstanceDeleted(dbInstanceIdentifier).Result;
                            }
                            // Finished restore
                            if (!ct.IsCancellationRequested) // Check task run is not canceled
                            {
                                _tenantRepositoryRunTask.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["available"]);
                                var messenge = $"{tenant.EndSubDomain} is restore successfully.";
                                var notification = _notificationRepositoryRunTask.CreateNotification(ConfigConstant.StatusNotiSuccess, messenge);

                                // Add info tenant for notification
                                notification.SetTenantId(tenant.TenantId);
                                notification.SetStatusTenant(tenant.StatusTenant);
                                _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, notification);
                            }
                            cts.Cancel();
                            return;
                        }
                    }

                    catch (Exception ex)
                    {
                        if (!ct.IsCancellationRequested) // Check task run is not canceled
                        {
                            _tenantRepository.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["restore-failed"]);
                            // Notification  restore failed
                            var messenge = $"{tenant.EndSubDomain} is restore failed. Error: {ex.Message}.";
                            var notification = _notificationRepositoryRunTask.CreateNotification(ConfigConstant.StatusNotifailure, messenge);
                            // Add info tenant for notification
                            notification.SetTenantId(tenant.TenantId);
                            notification.SetStatusTenant(tenant.StatusTenant);
                            _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, notification);
                        }
                        cts.Cancel();
                        return;
                    }
                    finally
                    {
                        _tenantRepositoryRunTask.ReleaseResource();
                        _notificationRepositoryRunTask.ReleaseResource();
                    }
                }, cts.Token);
                return new RestoreTenantOutputData(true, RestoreTenantStatus.Successed);
            }
            finally
            {
                _tenantRepository.ReleaseResource();
                _notificationRepository.ReleaseResource();
            }
        }

        /// <summary>
        /// Checking RDS restore available.
        /// </summary>
        /// <param name="dbInstanceIdentifier"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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
                            Console.WriteLine($"DB Instance status: {checkStatus}");
                        }

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
