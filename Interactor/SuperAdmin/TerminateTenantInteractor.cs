using AWSSDK.Common;
using AWSSDK.Constants;
using AWSSDK.Interfaces;
using Domain.SuperAdminModels.Tenant;
using Interactor.Realtime;
using Npgsql;
using System.Data.Common;
using UseCase.SuperAdmin.TerminateTenant;

namespace Interactor.SuperAdmin
{
    public class TerminateTenantInteractor : ITerminateTenantInputPort
    {
        private readonly IAwsSdkService _awsSdkService;
        private readonly ITenantRepository _tenantRepository;
        private readonly IWebSocketService _webSocketService;
        public TerminateTenantInteractor(ITenantRepository tenantRepository, IAwsSdkService awsSdkService, IWebSocketService webSocketService)
        {
            _awsSdkService = awsSdkService;
            _tenantRepository = tenantRepository;
            _webSocketService = webSocketService;
        }
        public TerminateTenantOutputData Handle(TerminateTenantInputData inputData)
        {
            if (inputData.TenantId <= 0)
            {
                return new TerminateTenantOutputData(false, TerminateTenantStatus.InvalidTenantId);
            }

            var tenant = _tenantRepository.Get(inputData.TenantId);
            if (tenant == null)
            {
                return new TerminateTenantOutputData(false, TerminateTenantStatus.InvalidTenantId);
            }

            _tenantRepository.UpdateStatusTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["terminating"]);
            var listTenantDb = RDSAction.GetListDatabase(tenant.RdsIdentifier).Result;

            // Check valid delete tennatDb
            if (listTenantDb == null || listTenantDb.Count() == 0 || !listTenantDb.Contains(tenant.Db))
            {
                return new TerminateTenantOutputData(false, TerminateTenantStatus.TenantDbDoesNotExistInRDS);
            }

            CancellationTokenSource cts = new CancellationTokenSource();
            _ = Task.Run(async () =>
            {
                Console.WriteLine($"Start Terminate tenant: {tenant.RdsIdentifier}");
                // Connect RDS delete TenantDb
                if (listTenantDb.Count > 1)
                {
                    if (DeleteTenantDb(tenant.EndPointDb, tenant.Db))
                    {

                    }
                    else
                    {

                    }

                }

                // Delete RDS
                // Delete DNS
                // Delete Could front
                else
                {
                    if (await RDSAction.DeleteRDSInstanceAsync(tenant.RdsIdentifier))
                    {
                    }

                    // Terminate failure 
                    else
                    {
                        _tenantRepository.TerminateTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["terminate-failed"]);
                    }

                    // Check Deleted RDS, DNS, Could front
                    if (await RDSAction.CheckRDSInstanceDeleted(tenant.RdsIdentifier))
                    {
                        _tenantRepository.TerminateTenant(inputData.TenantId, ConfigConstant.StatusTenantDictionary()["terminated"]);
                    }
                }

                
                // Notification

                //Finished terminate
                cts.Cancel();
                return;
            });
            return new TerminateTenantOutputData(true, TerminateTenantStatus.Successed);
        }

        public bool DeleteTenantDb(string serverEndpoint, string tennantDB)
        {
            try
            {
                // Replace these values with your actual RDS information
                string username = "postgres";
                string password = "Emr!23456789";
                var port = 5432;
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
                            command.CommandText = $"EXECUTE 'DROP DATABASE {tennantDB};";
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
    }
}
