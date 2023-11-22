using AWSSDK.Common;
using AWSSDK.Constants;
using AWSSDK.Interfaces;
using Domain.SuperAdminModels.Tenant;
using Interactor.Realtime;
using Npgsql;
using System.Data.Common;
using UseCase.SuperAdmin.UpgradePremium;

namespace Interactor.SuperAdmin
{
    public class UpgradePremiumInteractor : IUpgradePremiumInputPort
    {
        private readonly IAwsSdkService _awsSdkService;
        private readonly ITenantRepository _tenantRepository;
        private readonly IWebSocketService _webSocketService;
        public UpgradePremiumInteractor(ITenantRepository tenantRepository, IAwsSdkService awsSdkService, IWebSocketService webSocketService)
        {
            _awsSdkService = awsSdkService;
            _tenantRepository = tenantRepository;
            _webSocketService = webSocketService;
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

                if (tenant.Type == ConfigConstant.TypeDedicate)
                {
                    return new UpgradePremiumOutputData(false, UpgradePremiumStatus.FailedTenantIsPremium);
                }
                _tenantRepository.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["restoring"]);
                CancellationTokenSource cts = new CancellationTokenSource();
                _ = Task.Run(async () =>
                {
                    // Create SnapShot
                    var snapshotIdentifier = await _awsSdkService.CreateDBSnapshotAsync(tenant.RdsIdentifier);

                    if (string.IsNullOrEmpty(snapshotIdentifier) || !await RDSAction.CheckSnapshotAvailableAsync(snapshotIdentifier))
                    {
                        _tenantRepository.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["restor-failed"]);
                        await _webSocketService.SendMessageAsync(FunctionCodes.FailedUpgradePremium, tenant);
                        cts.Cancel();
                        return;
                    }

                    // Restore DB Instance from snapshot
                    Console.WriteLine($"Start Restore");

                    string rString = CommonConstants.GenerateRandomString(6);
                    var dbInstanceIdentifier = $"develop-smartkarte-logging-{rString}";
                    Console.WriteLine($"Start Restore: {dbInstanceIdentifier}");

                    var isSuccessRestoreInstance = await _awsSdkService.RestoreDBInstanceFromSnapshot(dbInstanceIdentifier, snapshotIdentifier);

                    // Check Restore success 
                    var endpoint = await RDSAction.CheckRestoredInstanceAvailableAsync(dbInstanceIdentifier);
                    if (!isSuccessRestoreInstance || endpoint == null || string.IsNullOrEmpty(endpoint?.Address))
                    {
                        _tenantRepository.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["restor-failed"]);
                        await _webSocketService.SendMessageAsync(FunctionCodes.FailedUpgradePremium, tenant);
                        cts.Cancel();
                        return;
                    } 

                    // Update endpoint, dbInstanceIdentifier, status tenant available 
                    var tenantUpgrade = _tenantRepository.UpgradePremium(inputData.TenantId, dbInstanceIdentifier, endpoint.Address);

                    // Finished upgrade
                    if (tenantUpgrade != null)
                    {
                        await _webSocketService.SendMessageAsync(FunctionCodes.FinishedUpgradePremium, tenantUpgrade);
                    }

                    //Delete list Db without tenant DB
                    var isDeleteSuccess = ConnectAndDeleteDatabases(endpoint.Address, endpoint.Port, tenant.Db);
                    if (!isDeleteSuccess)
                    {
                        // To rerun  delete
                    }

                    // Todo Delete DB in old RDS
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

        public bool ConnectAndDeleteDatabases(string serverEndpoint, int port, string tennantDB)
        {
            try
            {
                // Replace these values with your actual RDS information
                string username = "postgres";
                string password = "Emr!23456789";
                // Connection string format for SQL Server
                string connectionString = $"Host={serverEndpoint};Port={port};Username={username};Password={password};";

                // Create and open a connection
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        // Delete database
                        using (DbCommand command = connection.CreateCommand())
                        {
                            command.CommandText = @$"
                                            DO $$ 
                                            DECLARE 
                                                db_name text; 
                                            BEGIN 
                                                FOR db_name IN (SELECT datname FROM pg_catalog.pg_database WHERE datname != ${tennantDB}) 
                                                LOOP 
                                                    EXECUTE 'DROP DATABASE IF EXISTS ' || db_name; 
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
