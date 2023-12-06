using Amazon.RDS;
using Amazon.RDS.Model;
using AWSSDK.Common;
using AWSSDK.Constants;
using AWSSDK.Interfaces;
using Domain.SuperAdminModels.Notification;
using Domain.SuperAdminModels.Tenant;
using Helper.Redis;
using Interactor.Realtime;
using Microsoft.Extensions.Configuration;
using Npgsql;
using StackExchange.Redis;
using UseCase.SuperAdmin.UpgradePremium;

namespace Interactor.SuperAdmin
{
    public class UpgradePremiumInteractor : IUpgradePremiumInputPort
    {
        private readonly IAwsSdkService _awsSdkService;
        private readonly ITenantRepository _tenantRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly ITenantRepository _tenantRepositoryRunTask;
        private readonly INotificationRepository _notificationRepositoryRunTask;
        private readonly IConfiguration _configuration;
        private readonly IDatabase _cache;
        public UpgradePremiumInteractor(
            ITenantRepository tenantRepository,
            IAwsSdkService awsSdkService,
            INotificationRepository notificationRepository,
            ITenantRepository tenantRepositoryRunTask,
            INotificationRepository notificationRepositoryRunTask,
            IConfiguration configuration
            )
        {
            _awsSdkService = awsSdkService;
            _tenantRepository = tenantRepository;
            _notificationRepository = notificationRepository;
            _tenantRepositoryRunTask = tenantRepositoryRunTask;
            _notificationRepositoryRunTask = notificationRepositoryRunTask;
            _configuration = configuration;
            GetRedis();
            _cache = RedisConnectorHelper.Connection.GetDatabase();
        }

        private void GetRedis()
        {
            string connection = string.Concat(_configuration["Redis:RedisHost"], ":", _configuration["Redis:RedisPort"]);
            if (RedisConnectorHelper.RedisHost != connection)
            {
                RedisConnectorHelper.RedisHost = connection;
            }
        }

        public UpgradePremiumOutputData Handle(UpgradePremiumInputData inputData)
        {
            try
            {
                IWebSocketService _webSocketService;
                _webSocketService = (IWebSocketService)inputData.WebSocketService;
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
                if (oldTenant.TenantId <= 0 || oldTenant.TenantId <= 0)
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
                _ = Task.Run(() =>
                {
                    try
                    {
                        // Create SnapShot
                        var snapshotIdentifier = _awsSdkService.CreateDBSnapshotAsync(oldTenant.RdsIdentifier, ConfigConstant.RdsSnapshotUpgrade).Result;

                        if (string.IsNullOrEmpty(snapshotIdentifier) || !RDSAction.CheckSnapshotAvailableAsync(snapshotIdentifier).Result)
                        {
                            throw new Exception("Snapshot is not Available");
                        }

                        // Create New subdomain
                        if (oldTenant.SubDomain != inputData.SubDomain)
                        {
                            if (Route53Action.CreateTenantDomain(inputData.SubDomain).Result != null)
                            {
                                var actionDeleteDomain = Route53Action.DeleteTenantDomain(oldTenant.SubDomain).Result;
                            }
                            else
                            {
                                throw new Exception("Create New subdomain failed");
                            }
                        }

                        // Restore DB Instance from snapshot
                        Console.WriteLine($"Start Restore");

                        string rString = CommonConstants.GenerateRandomString(6);
                        var dbInstanceIdentifier = $"{inputData.SubDomain}-{rString}";
                        Console.WriteLine($"Start Restore: {dbInstanceIdentifier}");

                        var isSuccessRestoreInstance = _awsSdkService.RestoreDBInstanceFromSnapshot(dbInstanceIdentifier, snapshotIdentifier).Result;

                        // Check Restore success 
                        var endpoint = CheckRestoredInstanceAvailableAsync(dbInstanceIdentifier, inputData.TenantId).Result;


                        //Delete list Db without tenantDB in new RDS
                        Console.WriteLine($"Start Terminate old tenant: {oldTenant.RdsIdentifier}");
                        var isDeleteSuccess = ConnectAndDeleteDatabases(endpoint.Address, oldTenant.Db);


                        // Delete DB in old RDS
                        var listTenantDb = RDSAction.GetListDatabase(oldTenant.EndPointDb).Result;
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
                            var actionDeleteRDS = RDSAction.DeleteRDSInstanceAsync(oldTenant.RdsIdentifier);
                        }

                        // Update endpoint, dbInstanceIdentifier, status tenant available 
                        var tenantUpgrade = _tenantRepositoryRunTask.UpgradePremium(inputData.TenantId, dbInstanceIdentifier, endpoint.Address, inputData.SubDomain, inputData.Size, inputData.SizeType);

                        // Finished upgrade
                        if (tenantUpgrade != null)
                        {
                            // set cache to tenantId
                            var key = "cache_tenantId_" + tenantUpgrade.SubDomain;
                            if (_cache.KeyExists(key))
                            {
                                _cache.KeyDelete(key);
                                _cache.StringSet(key, tenantUpgrade.TenantId.ToString());
                            }
                            var messenge = $"{oldTenant.EndSubDomain} is upgrade premium successfully.";
                            var notification = _notificationRepositoryRunTask.CreateNotification(ConfigConstant.StatusTenantDictionary()["available"], messenge);
                            _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, notification);
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
                        _tenantRepositoryRunTask.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["upgrade-failed"]);
                        var messenge = $"{oldTenant.EndSubDomain} is upgrade premium failed. Error: {ex.Message}.";
                        var notification = _notificationRepositoryRunTask.CreateNotification(ConfigConstant.StatusNotifailure, messenge);
                        _webSocketService.SendMessageAsync(FunctionCodes.SuperAdmin, notification);
                        cts.Cancel();
                        return;
                    }
                    finally
                    {
                        _tenantRepositoryRunTask.ReleaseResource();
                        _notificationRepositoryRunTask.ReleaseResource();
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

        public bool ConnectAndDeleteDatabases(string serverEndpoint, string tennantDB)
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
                                _tenantRepositoryRunTask.UpdateStatusTenant(tenantId, statusTenant);
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
