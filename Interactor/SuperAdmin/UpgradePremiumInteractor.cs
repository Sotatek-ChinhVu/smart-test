using Amazon.RDS.Model;
using Amazon.RDS;
using AWSSDK.Common;
using AWSSDK.Constants;
using AWSSDK.Interfaces;
using Domain.SuperAdminModels.Tenant;
using Interactor.Realtime;
using Npgsql;
using UseCase.SuperAdmin.UpgradePremium;
using Entity.SuperAdmin;

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
                if (oldTenant.TenantId <= 0 )
                {
                    return new UpgradePremiumOutputData(false, UpgradePremiumStatus.TenantDoesNotExist);
                }

                var listTenantDb = RDSAction.GetListDatabase(oldTenant.RdsIdentifier);

                if (oldTenant.Type == ConfigConstant.TypeDedicate)
                {
                    return new UpgradePremiumOutputData(false, UpgradePremiumStatus.FailedTenantIsPremium);
                }

                if (!_awsSdkService.CheckExitRDS(oldTenant.RdsIdentifier).Result)
                {
                    return new UpgradePremiumOutputData(false, UpgradePremiumStatus.RdsDoesNotExist);
                }

                //if (oldTenant.Size != inputData.Size || oldTenant.SizeType != inputData.SizeType)
                //{
                //    // check valid
                //}

                if (oldTenant.SubDomain != inputData.SubDomain)
                {
                    if (Route53Action.CheckSubdomainExistence(inputData.SubDomain).Result)
                    {
                        return new UpgradePremiumOutputData(false, UpgradePremiumStatus.NewDomainAleadyExist);
                    }
                }

                _tenantRepository.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["restoring"]);
                CancellationTokenSource cts = new CancellationTokenSource();
                _ = Task.Run(async () =>
                {
                    // Create SnapShot
                    var snapshotIdentifier = await _awsSdkService.CreateDBSnapshotAsync(oldTenant.RdsIdentifier);

                    if (string.IsNullOrEmpty(snapshotIdentifier) || !await RDSAction.CheckSnapshotAvailableAsync(snapshotIdentifier))
                    {
                        _tenantRepository.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["restore-failed"]);
                        _notificationRepository.CreateNotification(ConfigConstant.StatusTenantDictionary()["restore-failed"], FunctionCodes.FailedUpgradePremium);
                        await _webSocketService.SendMessageAsync(FunctionCodes.FailedUpgradePremium, oldTenant);
                        cts.Cancel();
                        return;
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
                            _tenantRepository.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["restore-failed"]);
                            _notificationRepository.CreateNotification(ConfigConstant.StatusTenantDictionary()["restore-failed"], FunctionCodes.FailedUpgradePremium);
                            await _webSocketService.SendMessageAsync(FunctionCodes.FailedUpgradePremium, oldTenant);
                            cts.Cancel();
                            return;
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
                    if (!isSuccessRestoreInstance || endpoint == null || string.IsNullOrEmpty(endpoint?.Address))
                    {
                        _tenantRepository.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["restore-failed"]);
                        _notificationRepository.CreateNotification(ConfigConstant.StatusTenantDictionary()["restore-failed"], FunctionCodes.FailedUpgradePremium);
                        await _webSocketService.SendMessageAsync(FunctionCodes.FailedUpgradePremium, oldTenant);
                        cts.Cancel();
                        return;
                    }

                    // Update endpoint, dbInstanceIdentifier, status tenant available 
                    var tenantUpgrade = _tenantRepository.UpgradePremium(inputData.TenantId, dbInstanceIdentifier, endpoint.Address, inputData.SubDomain, inputData.Size, inputData.SizeType);

                    // Finished upgrade
                    if (tenantUpgrade != null)
                    {
                        _notificationRepository.CreateNotification(ConfigConstant.StatusTenantDictionary()["available"], FunctionCodes.FinishedUpgradePremium);
                        await _webSocketService.SendMessageAsync(FunctionCodes.FinishedUpgradePremium, tenantUpgrade);
                    }

                    //Delete list Db without tenantDB in new RDS
                    Console.WriteLine($"Start Terminate old tenant: {oldTenant.RdsIdentifier}");
                    var isDeleteSuccess = ConnectAndDeleteDatabases(endpoint.Address, oldTenant.Db);

                    //if (!isDeleteSuccess)
                    //{
                    //    // To rerun  delete
                    //}

                    // Delete DB in old RDS
                    var listTenantDb = await RDSAction.GetListDatabase(oldTenant.RdsIdentifier);

                    // Connect RDS delete TenantDb
                    if (listTenantDb.Count > 1)
                    {
                        _awsSdkService.DeleteTenantDb(oldTenant.EndPointDb, oldTenant.Db);
                    }

                    // Deleted RDS
                    else
                    {
                        await RDSAction.DeleteRDSInstanceAsync(oldTenant.RdsIdentifier);
                    }

                    cts.Cancel();
                    return;
                });

                return new UpgradePremiumOutputData(true, UpgradePremiumStatus.Successed);
            }
            finally
            {
                _tenantRepository.ReleaseResource();
            }
        }

        public bool ConnectAndDeleteDatabases(string serverEndpoint, string tennantDB)
        {
            try
            {
                // Replace these values with your actual RDS information
                string username = "postgres";
                string password = "Emr!23456789";
                int port = 22;
                serverEndpoint = "localhost";

                // Connection string format for SQL Server
                string connectionString = $"Host={serverEndpoint};Port={port};Username={username};Password={password};";
                var withOutDb = ConfigConstant.LISTSYSTEMDB;
                withOutDb.Add(tennantDB);
                string strWithoutDb = string.Join(", ", withOutDb);
                strWithoutDb = "'" + strWithoutDb.Replace(", ", "', '") + "'";

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
                            command.CommandText = @$"
                                                        DO $$ 
                                                        DECLARE 
                                                            db_name text; 
                                                        BEGIN 
                                                            FOR db_name IN (SELECT datname FROM pg_catalog.pg_database WHERE datname NOT IN ({strWithoutDb}) AND NOT datistemplate) 
                                                            LOOP 
                                                                EXECUTE 'DROP DATABASE IF EXISTS ' || quote_ident(db_name); 
                                                            END LOOP; 
                                                        END $$;
                                                        ";
                            command.ExecuteNonQuery();
                        }

                        Console.WriteLine($"Database deleted successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public  async Task<Endpoint> CheckRestoredInstanceAvailableAsync(string dbInstanceIdentifier, int tenantId)
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
                        return new Endpoint();
                    }

                    // Check if more than timeout
                    if ((DateTime.Now - startTime).TotalMinutes > ConfigConstant.TimeoutCheckingAvailable)
                    {
                        Console.WriteLine($"Timeout: DB instance not available after {ConfigConstant.TimeoutCheckingAvailable} minutes.");
                        running = false;
                        return new Endpoint();
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
                return new Endpoint();
            }
        }
    }
}
