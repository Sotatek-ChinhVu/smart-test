using Amazon.RDS;
using Amazon.RDS.Model;
using AWSSDK.Common;
using AWSSDK.Constants;
using AWSSDK.Interfaces;
using Domain.SuperAdminModels.Notification;
using Domain.SuperAdminModels.Tenant;
using Interactor.Realtime;
using Npgsql;
using UseCase.SuperAdmin.UpgradePremium;

namespace Interactor.SuperAdmin
{
    public class UpgradePremiumInteractor : IUpgradePremiumInputPort
    {
        private readonly IAwsSdkService _awsSdkService;
        private readonly ITenantRepository _tenantRepository;
        private readonly IWebSocketService _webSocketService;
        private readonly INotificationRepository _notificationRepository;
        public UpgradePremiumInteractor(ITenantRepository tenantRepository, IAwsSdkService awsSdkService, IWebSocketService webSocketService, INotificationRepository notificationRepository)
        {
            _awsSdkService = awsSdkService;
            _tenantRepository = tenantRepository;
            _webSocketService = webSocketService;
            _notificationRepository = notificationRepository;
        }

        public UpgradePremiumOutputData Handle(UpgradePremiumInputData inputData)
        {
            try
            {
                if (inputData.TenantId <= 0)
                {
                    return new UpgradePremiumOutputData(false, UpgradePremiumStatus.InvalidTenantId);
                }

                if (inputData.Size <= 0)
                {
                    return new UpgradePremiumOutputData(false, UpgradePremiumStatus.InvalidSize);
                }

                if (inputData.SizeType <= 0)
                {
                    return new UpgradePremiumOutputData(false, UpgradePremiumStatus.InvalidSizeType);
                }

                if (string.IsNullOrEmpty(inputData.SubDomain))
                {
                    return new UpgradePremiumOutputData(false, UpgradePremiumStatus.InvalidDomain);
                }

                var oldTenant = _tenantRepository.Get(inputData.TenantId);
                if (oldTenant.TenantId <= 0)
                {
                    return new UpgradePremiumOutputData(false, UpgradePremiumStatus.TenantDoesNotExist);
                }

                if (oldTenant.Type == ConfigConstant.TypeDedicate)
                {
                    return new UpgradePremiumOutputData(false, UpgradePremiumStatus.FailedTenantIsPremium);
                }

                if (!_awsSdkService.CheckExitRDS(oldTenant.RdsIdentifier).Result)
                {
                    return new UpgradePremiumOutputData(false, UpgradePremiumStatus.RdsDoesNotExist);
                }

                if (oldTenant.SubDomain != inputData.SubDomain)
                {
                    if (Route53Action.CheckSubdomainExistence(inputData.SubDomain).Result)
                    {
                        return new UpgradePremiumOutputData(false, UpgradePremiumStatus.NewDomainAleadyExist);
                    }
                }
                _tenantRepository.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["upgrading"]);
                CancellationTokenSource cts = new CancellationTokenSource();
                _ = Task.Run(async () =>
                {
                    try
                    {
                        // Create SnapShot
                        var snapshotIdentifier = await _awsSdkService.CreateDBSnapshotAsync(oldTenant.RdsIdentifier);

                        if (string.IsNullOrEmpty(snapshotIdentifier) || !await RDSAction.CheckSnapshotAvailableAsync(snapshotIdentifier))
                        {
                            throw new Exception("Snapshot is not Available");
                        }

                        // Create New subdomain
                        if (oldTenant.SubDomain != inputData.SubDomain)
                        {
                            if (await Route53Action.CreateTenantDomain(inputData.SubDomain) != null)
                            {
                                await Route53Action.DeleteTenantDomain(oldTenant.SubDomain);
                            }
                            else
                            {
                                throw new Exception("Create New subdomain failed");
                            }
                        }

                        // Restore DB Instance from snapshot
                        Console.WriteLine($"Start Restore");

                        string rString = CommonConstants.GenerateRandomString(6);
                        var dbInstanceIdentifier = $"develop-smartkarte-logging-{rString}";
                        Console.WriteLine($"Start Restore: {dbInstanceIdentifier}");

                        var isSuccessRestoreInstance = await _awsSdkService.RestoreDBInstanceFromSnapshot(dbInstanceIdentifier, snapshotIdentifier);

                        // Check Restore success 
                        var endpoint = await CheckRestoredInstanceAvailableAsync(dbInstanceIdentifier, inputData.TenantId);


                        //Delete list Db without tenantDB in new RDS
                        Console.WriteLine($"Start Terminate old tenant: {oldTenant.RdsIdentifier}");
                        var isDeleteSuccess = ConnectAndDeleteDatabases(endpoint.Address, oldTenant.Db);


                        // Delete DB in old RDS
                        var listTenantDb = await RDSAction.GetListDatabase(oldTenant.EndPointDb);
                        Console.WriteLine($"listTenantDb: {listTenantDb}");
                        // Connect RDS delete TenantDb
                        if (listTenantDb.Count > 1)
                        {
                            Console.WriteLine($"Connect RDS delete TenantDb: {oldTenant.RdsIdentifier}");
                            _awsSdkService.DeleteTenantDb(oldTenant.EndPointDb, oldTenant.Db);
                        }

                        // Deleted RDS
                        else
                        {
                            Console.WriteLine($"Deleted RDS: {oldTenant.RdsIdentifier}");
                            await RDSAction.DeleteRDSInstanceAsync(oldTenant.RdsIdentifier);
                        }

                        // Update endpoint, dbInstanceIdentifier, status tenant available 
                        var tenantUpgrade = _tenantRepository.UpgradePremium(inputData.TenantId, dbInstanceIdentifier, endpoint.Address, inputData.SubDomain, inputData.Size, inputData.SizeType);

                        // Finished upgrade
                        if (tenantUpgrade != null)
                        {
                            var messenge = $"{oldTenant.EndSubDomain} is upgrade premium successfully.";
                            var notification = _notificationRepository.CreateNotification(ConfigConstant.StatusTenantDictionary()["available"], messenge);
                            await _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, notification);
                            cts.Cancel();
                            return;
                        }
                        else
                        {
                            throw new Exception("Update new data tenant failed");
                        }
                    }
                    catch (Exception ex)
                    {
                        _tenantRepository.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["upgrade-failed"]);
                        // Notification  upgrade failed
                        _tenantRepository.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["upgrade-failed"]);
                        var messenge = $"{oldTenant.EndSubDomain} is upgrade premium failed. Error: {ex.Message}.";
                        var notification = _notificationRepository.CreateNotification(ConfigConstant.StatusNotifailure, messenge);
                        await _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, notification);
                        cts.Cancel();
                        return;
                    }
                });

                return new UpgradePremiumOutputData(true, UpgradePremiumStatus.Successed);
            }
            finally
            {
                _tenantRepository.ReleaseResource();
                _notificationRepository.ReleaseResource();
            }
        }

        public  bool ConnectAndDeleteDatabases(string serverEndpoint, string tennantDB)
        {
            try
            {
                // Replace these values with your actual RDS information
                string username = "postgres";
                string password = "Emr!23456789";
                int port = 5432;

                // Connection string format for SQL Server
                string connectionString = $"Host={serverEndpoint};Port={port};Username={username};Password={password};";
                var listTenantDb = RDSAction.GetListDatabase(serverEndpoint).Result;
                if (listTenantDb.Contains(tennantDB))
                {
                    listTenantDb.Remove(tennantDB);
                }
                else
                {
                    throw new Exception($"Connec tAndDelete Databases. tennantDB doesn't exists");
                }

                // Create and open a connection
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        // Delete database
                        using (NpgsqlCommand command = new NpgsqlCommand())
                        {
                            command.Connection = connection;
                            foreach (var item in listTenantDb)
                            {
                                command.CommandText += $"DROP DATABASE {item};";
                            }
                            command.ExecuteNonQuery();
                        }

                        Console.WriteLine($"Database deleted successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        throw new Exception($"Connect And Delete Databases Failed. {ex.Message}");
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new Exception($"Delete redundant Databases in new RDS. {ex.Message}");
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
