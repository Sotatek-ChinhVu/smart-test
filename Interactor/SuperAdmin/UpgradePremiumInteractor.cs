using AWSSDK.Common;
using AWSSDK.Constants;
using AWSSDK.Interfaces;
using Domain.SuperAdminModels.Tenant;
using System.Data.Common;
using System.Data.SqlClient;
using UseCase.SuperAdmin.UpgradePremium;

namespace Interactor.SuperAdmin
{
    public class UpgradePremiumInteractor : IUpgradePremiumInputPort
    {
        private readonly IAwsSdkService _awsSdkService;
        private readonly ITenantRepository _tenantRepository;
        public UpgradePremiumInteractor(ITenantRepository tenantRepository, IAwsSdkService awsSdkService)
        {
            _awsSdkService = awsSdkService;
            _tenantRepository = tenantRepository;
        }

        public UpgradePremiumOutputData Handle(UpgradePremiumInputData inputData)
        {
            try
            {
                if (inputData.TenantId <= 0)
                {
                    return new UpgradePremiumOutputData(false, UpgradePremiumStatus.InvalidTenantId);
                }

                var tenant = _tenantRepository.Get(inputData.TenantId);

                if (tenant.Type == 1)
                {
                    return new UpgradePremiumOutputData(false, UpgradePremiumStatus.FailedTenantIsPremium);
                }

                // Check exit domain 
                var checkSubDomain = _awsSdkService.CheckSubdomainExistenceAsync(tenant.SubDomain).Result;
                if (checkSubDomain)
                {
                    return new UpgradePremiumOutputData(false, UpgradePremiumStatus.FailedTenantIsPremium);
                }

                CancellationTokenSource cts = new CancellationTokenSource();
                _ = Task.Run(async () =>
                {
                    // Create SnapShot
                    var snapshotIdentifier = await _awsSdkService.CreateDBSnapshotAsync("develop-smartkarte-logging");

                    if (string.IsNullOrEmpty(snapshotIdentifier))
                    {
                        cts.Cancel();
                    }

                    var isAvailableSnapShot = await RDSAction.CheckingSnapshotAvailableAsync(snapshotIdentifier);
                    if (!isAvailableSnapShot)
                    {
                        cts.Cancel();
                    }

                    // Restore DB Instance from snapshot
                    Console.WriteLine($"Start Restore");

                    string rString = CommonConstants.GenerateRandomString(6);
                    var dbInstanceIdentifier = $"develop-smartkarte-postgres-{rString}";
                    Console.WriteLine($"Start Restore: {dbInstanceIdentifier}");

                   var endpoint =  await _awsSdkService.RestoreDBInstanceFromSnapshot(dbInstanceIdentifier, snapshotIdentifier);
                    if (endpoint == null)
                    {
                        cts.Cancel();
                    }
                    // Check Restore success 
                    var isAvailableRestoreInstance = await RDSAction.CheckRestoredInstanceAvailableAsync(dbInstanceIdentifier);
                    if (!isAvailableRestoreInstance)
                    {
                        cts.Cancel();
                    }
                    // Get list DB from Instance
                    var databaseList = await RDSAction.GetDatabasesFromDBInstanceAsync(dbInstanceIdentifier);
                    if (databaseList.Contains(tenant.Db))
                    {
                        databaseList.Remove(tenant.Db);
                    }
                    else
                    {
                        cts.Cancel();
                    }
                    // Delete list Db without tenant DB
                    ConnectAndDeleteDatabases(endpoint.Address, endpoint.Port, databaseList);
                    // Update  endpoint 
                });

                return new UpgradePremiumOutputData(true, UpgradePremiumStatus.Successed);
            }
            finally
            {
                _tenantRepository.ReleaseResource();
            }
        }

        public bool ConnectAndDeleteDatabases(string serverEndpoint, int port, List<string> databaseNames)
        {
            try
            {
                // Replace these values with your actual RDS information
                string username = "YourUsername";
                string password = "YourPassword";

                // Connection string format for SQL Server
                string connectionString = $"Server={serverEndpoint},{port};User Id={username};Password={password};";

                // Create and open a connection
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        foreach (var databaseName in databaseNames)
                        {
                            // Change database
                            connection.ChangeDatabase(databaseName);

                            // Delete database
                            using (DbCommand command = connection.CreateCommand())
                            {
                                command.CommandText = $"DROP DATABASE [{databaseName}]";
                                command.ExecuteNonQuery();
                            }

                            Console.WriteLine($"Database '{databaseName}' deleted successfully.");
                        }

                        Console.WriteLine("Connected to the database.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
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
    }
}
