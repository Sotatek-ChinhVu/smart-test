using Amazon.RDS.Model;
using Amazon.RDS;
using AWSSDK.Common;
using AWSSDK.Constants;
using AWSSDK.Interfaces;
using Domain.SuperAdminModels.Tenant;
using UseCase.SuperAdmin.TenantOnboard;
using Domain.SuperAdminModels.MigrationTenantHistory;

namespace Interactor.SuperAdmin
{
    public class TenantOnboardInteractor : ITenantOnboardInputPort
    {
        private readonly IAwsSdkService _awsSdkService;
        private readonly ITenantRepository _tenantRepository;
        private readonly IMigrationTenantHistoryRepository _migrationTenantHistoryRepository;
        public TenantOnboardInteractor(IAwsSdkService awsSdkService, ITenantRepository tenantRepository, IMigrationTenantHistoryRepository migrationTenantHistoryRepository)
        {
            _awsSdkService = awsSdkService;
            _tenantRepository = tenantRepository;
            _migrationTenantHistoryRepository = migrationTenantHistoryRepository;
        }
        public TenantOnboardOutputData Handle(TenantOnboardInputData inputData)
        {
            try
            {
                if (inputData.Size <= 0)
                {
                    return new TenantOnboardOutputData(new(), TenantOnboardStatus.InvalidSize);
                }
                else if (inputData.SizeType != ConfigConstant.SizeTypeMB && inputData.SizeType != ConfigConstant.SizeTypeGB)
                {
                    return new TenantOnboardOutputData(new(), TenantOnboardStatus.InvalidSizeType);
                }
                else if (inputData.ClusterMode != ConfigConstant.TypeSharing && inputData.ClusterMode != ConfigConstant.TypeDedicate)
                {
                    return new TenantOnboardOutputData(new(), TenantOnboardStatus.InvalidClusterMode);
                }
                var checkSubDomain = _awsSdkService.CheckSubdomainExistenceAsync(inputData.SubDomain).Result;
                if (checkSubDomain)
                {
                    return new TenantOnboardOutputData(new(), TenantOnboardStatus.SubDomainExists);
                }
                var tenantModel = new TenantModel(inputData.Hospital, 0, inputData.AdminId, inputData.Password, inputData.SubDomain, inputData.SubDomain, inputData.Size, inputData.SizeType, inputData.ClusterMode, string.Empty, string.Empty, 0, string.Empty, inputData.SubDomain, CommonConstants.GenerateRandomString(6));
                var tenantOnboard = TenantOnboardAsync(tenantModel).Result;
                var message = string.Empty;
                if (tenantOnboard.TryGetValue("Error", out string? errorValue))
                {
                    Console.WriteLine($"Exception: {errorValue}");
                    return new TenantOnboardOutputData(new(), TenantOnboardStatus.Failed);
                }
                if (tenantOnboard.TryGetValue("message", out string? messageValue))
                {
                    message = messageValue;
                }
                var data = new TenantOnboardItem(message);

                return new TenantOnboardOutputData(data, TenantOnboardStatus.Successed);
            }
            finally
            {
                _tenantRepository.ReleaseResource();
            }
        }

        private async Task<string> CheckingRDSStatusAsync(string dbIdentifier, int tenantId, string tenantUrl)
        {
            try
            {
                string host = string.Empty;
                bool running = true;
                string status = string.Empty;
                DateTime startTime = DateTime.Now;

                while (running)
                {
                    var rdsClient = new AmazonRDSClient();

                    var response = await rdsClient.DescribeDBInstancesAsync(new DescribeDBInstancesRequest
                    {
                        DBInstanceIdentifier = dbIdentifier
                    });

                    var dbInstances = response.DBInstances;

                    if (dbInstances.Count != 1)
                    {
                        throw new Exception("More than one Database Shard returned; this should never happen");
                    }

                    var dbInstance = dbInstances[0];
                    var checkStatus = dbInstance.DBInstanceStatus;
                    if (status != checkStatus)
                    {
                        status = checkStatus;
                        var rdsStatusDictionary = ConfigConstant.StatusTenantDictionary();
                        if (rdsStatusDictionary.TryGetValue(checkStatus, out byte statusTenant))
                        {
                            var updateStatus = _tenantRepository.UpdateInfTenant(tenantId, statusTenant, string.Empty, string.Empty, dbIdentifier);
                        }
                    }

                    Console.WriteLine($"Last Database Shard status: {checkStatus}");
                    Thread.Sleep(5000);

                    if (checkStatus == "available")
                    {
                        var endpoint = dbInstance.Endpoint;
                        host = endpoint.Address;
                        // update status available: 1
                        var updateStatus = _tenantRepository.UpdateInfTenant(tenantId, 1, tenantUrl, host, dbIdentifier);
                        running = false;
                    }
                    // Check if more than timeout
                    if ((DateTime.Now - startTime).TotalMinutes > ConfigConstant.TimeoutCheckingAvailable)
                    {
                        Console.WriteLine($"Timeout: DB instance not available after {ConfigConstant.TimeoutCheckingAvailable} minutes.");
                        running = false;
                    }
                }

                return host;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
        private async Task<Dictionary<string, string>> TenantOnboardAsync(TenantModel model)
        {
            string subDomain = model.SubDomain;
            int size = model.Size;
            int sizeType = model.SizeType;
            int tier = model.Type;
            string rString = CommonConstants.GenerateRandomString(6);
            string tenantUrl = "";
            string host = "";

            try
            {
                // Provisioning SubDomain for new tenants
                if (!string.IsNullOrEmpty(subDomain))
                {
                    tenantUrl = $"{subDomain}.{ConfigConstant.Domain}";
                    await Route53Action.CreateTenantDomain(subDomain);
                    await CloudFrontAction.UpdateNewTenantAsync(subDomain);
                    // Checking Available RDS Cluster
                    if (subDomain.Length > 0)
                    {
                        // Checking tenant tier, if dedicated, provision new RDS instance
                        if (tier == ConfigConstant.TypeDedicate)
                        {
                            string dbIdentifier = $"develop-smartkarte-postgres-{rString}";
                            var rdsInfo = await RDSAction.GetRDSInformation();
                            if (rdsInfo.ContainsKey(dbIdentifier))
                            {
                                _ = Task.Run(async () =>
                                {
                                    var id = _tenantRepository.CreateTenant(model);
                                    model.ChangeRdsIdentifier(dbIdentifier);
                                    host = await CheckingRDSStatusAsync(dbIdentifier, id, tenantUrl);
                                    if (!string.IsNullOrEmpty(host))
                                    {
                                        var dataMigration = _migrationTenantHistoryRepository.GetMigration(id);
                                        RDSAction.CreateDatabase(host, subDomain, model.PasswordConnect);
                                        RDSAction.CreateTables(host, subDomain, dataMigration);
                                    }

                                });
                            }
                            else
                            {
                                var id = _tenantRepository.CreateTenant(model);
                                await RDSAction.CreateNewShardAsync(dbIdentifier);
                                model.ChangeRdsIdentifier(dbIdentifier);
                                _ = Task.Run(async () =>
                                {
                                    host = await CheckingRDSStatusAsync(dbIdentifier, id, tenantUrl);
                                    if (!string.IsNullOrEmpty(host))
                                    {
                                        var dataMigration = _migrationTenantHistoryRepository.GetMigration(id);
                                        RDSAction.CreateDatabase(host, subDomain, model.PasswordConnect);
                                        RDSAction.CreateTables(host, subDomain, dataMigration);
                                    }
                                });


                            }
                        }
                        else // In the rest cases, checking available RDS for new Tenant
                        {
                            var card = await CloudWatchAction.GetSummaryCardAsync();
                            List<string> availableIdentifier = card.Keys.Where(ids => card[ids]["available"] == "yes").ToList();

                            // If there's not any available RDS Cluster, provision new RDS cluster
                            if (availableIdentifier.Count == 0)
                            {
                                string dbIdentifier = $"develop-smartkarte-postgres-{rString}";
                                var id = _tenantRepository.CreateTenant(model);
                                await RDSAction.CreateNewShardAsync(dbIdentifier);
                                model.ChangeRdsIdentifier(dbIdentifier);
                                _ = Task.Run(async () =>
                                {
                                    host = await CheckingRDSStatusAsync(dbIdentifier, id, tenantUrl);
                                    if (!string.IsNullOrEmpty(host))
                                    {
                                        var dataMigration = _migrationTenantHistoryRepository.GetMigration(id);
                                        RDSAction.CreateDatabase(host, subDomain, model.PasswordConnect);
                                        RDSAction.CreateTables(host, subDomain, dataMigration);
                                    }
                                });
                            }
                            else // Else, returning the first available RDS Cluster in the list
                            {
                                bool checkAvailableIdentifier = false;
                                foreach (var dbIdentifier in availableIdentifier)
                                {
                                    var sumSubDomainToDbIdentifier = _tenantRepository.SumSubDomainToDbIdentifier(dbIdentifier);
                                    if (sumSubDomainToDbIdentifier <= 3)
                                    {
                                        checkAvailableIdentifier = true;
                                        model.ChangeRdsIdentifier(dbIdentifier);
                                        _ = Task.Run(async () =>
                                        {
                                            var id = _tenantRepository.CreateTenant(model);
                                            host = await CheckingRDSStatusAsync(dbIdentifier, id, tenantUrl);
                                            if (!string.IsNullOrEmpty(host))
                                            {
                                                var dataMigration = _migrationTenantHistoryRepository.GetMigration(id);
                                                RDSAction.CreateDatabase(host, subDomain, model.PasswordConnect);
                                                RDSAction.CreateTables(host, subDomain, dataMigration);
                                            }
                                        });
                                        break;
                                    }
                                }
                                if (!checkAvailableIdentifier)
                                {
                                    string dbIdentifierNew = $"develop-smartkarte-postgres-{rString}";
                                    var id = _tenantRepository.CreateTenant(model);
                                    await RDSAction.CreateNewShardAsync(dbIdentifierNew);
                                    model.ChangeRdsIdentifier(dbIdentifierNew);
                                    _ = Task.Run(async () =>
                                    {
                                        host = await CheckingRDSStatusAsync(dbIdentifierNew, id, tenantUrl);
                                        if (!string.IsNullOrEmpty(host))
                                        {
                                            var dataMigration = _migrationTenantHistoryRepository.GetMigration(id);
                                            RDSAction.CreateDatabase(host, subDomain, model.PasswordConnect);
                                            RDSAction.CreateTables(host, subDomain, dataMigration);
                                        }
                                    });
                                }
                            }
                        }
                    }
                }
                else // Return landing page url by default
                {
                    tenantUrl = "landingpage.smartkarte.org";
                }

                // Return message for Super Admin
                Dictionary<string, string> result = new Dictionary<string, string>
            {
                { "message", "Please wait for 15 minutes for all resources to be available" }
            };

                return result;
            }
            catch (Exception ex)
            {
                return new Dictionary<string, string> { { "Error", ex.Message } };
            }
        }
    }
}
