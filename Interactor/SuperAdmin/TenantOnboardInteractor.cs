using AWSSDK.Common;
using AWSSDK.Constants;
using AWSSDK.Interfaces;
using Domain.SuperAdminModels.Tenant;
using UseCase.SuperAdmin.TenantOnboard;

namespace Interactor.SuperAdmin
{
    public class TenantOnboardInteractor : ITenantOnboardInputPort
    {
        private readonly IAwsSdkService _awsSdkService;
        private readonly ITenantRepository _tenantRepository;
        public TenantOnboardInteractor(IAwsSdkService awsSdkService, ITenantRepository tenantRepository)
        {
            _awsSdkService = awsSdkService;
            _tenantRepository = tenantRepository;
        }
        public TenantOnboardOutputData Handle(TenantOnboardInputData inputData)
        {
            try
            {
                if (inputData.Size <= 0)
                {
                    return new TenantOnboardOutputData(new(), TenantOnboardStatus.InvalidSize);
                }
                else if (inputData.SizeType != 1 && inputData.SizeType != 2)
                {
                    return new TenantOnboardOutputData(new(), TenantOnboardStatus.InvalidSizeType);
                }
                else if (inputData.ClusterMode != 1 && inputData.ClusterMode != 2)
                {
                    return new TenantOnboardOutputData(new(), TenantOnboardStatus.InvalidClusterMode);
                }
                var checkSubDomain = _awsSdkService.CheckSubdomainExistenceAsync(inputData.SubDomain).Result;
                if (checkSubDomain)
                {
                    return new TenantOnboardOutputData(new(), TenantOnboardStatus.Failed);
                }
                var tenantOnboard = TenantOnboardAsync(inputData.SubDomain, inputData.Size, inputData.SizeType, inputData.ClusterMode).Result;
                var tenantUrl = string.Empty;
                var rdsEndpoint = string.Empty;
                var message = string.Empty;
                if (tenantOnboard.TryGetValue("Error", out string? errorValue))
                {
                    Console.WriteLine($"Exception: {errorValue}");
                    return new TenantOnboardOutputData(new(), TenantOnboardStatus.Failed);
                }
                if (tenantOnboard.TryGetValue("tenant_url", out string? tenantUrlValue))
                {
                    tenantUrl = tenantUrlValue;
                }
                if (tenantOnboard.TryGetValue("rds_endpoint", out string? rdsEndpointValue))
                {
                    rdsEndpoint = rdsEndpointValue;
                }
                if (tenantOnboard.TryGetValue("message", out string? messageValue))
                {
                    message = messageValue;
                }
                var data = new TenantOnboardItem(message, rdsEndpoint, tenantUrl);

                return new TenantOnboardOutputData(data, TenantOnboardStatus.Successed);
            }
            finally
            {
                _tenantRepository.ReleaseResource();
            }
        }

        public async Task<Dictionary<string, string>> TenantOnboardAsync(string tenantId, int size, int sizeType, int tier)
        {
            string rString = CommonConstants.GenerateRandomString(6);
            string tenantUrl = "";
            string host = "";

            try
            {
                // Provisioning SubDomain for new tenants
                if (!string.IsNullOrEmpty(tenantId))
                {
                    tenantUrl = $"{tenantId}.{ConfigConstant.Domain}";
                    await Route53Action.CreateTenantDomain(tenantId);
                    await CloudFrontAction.UpdateNewTenantAsync(tenantId);
                    // Checking Available RDS Cluster
                    if (tenantId.Length > 0)
                    {
                        // Checking tenant tier, if dedicated, provision new RDS instance
                        if (tier == 2)
                        {
                            string dbIdentifier = $"develop-smartkarte-postgres-{rString}";
                            var rdsInfo = await RDSAction.GetRDSInformation();
                            if (rdsInfo.ContainsKey(dbIdentifier))
                            {
                                host = await RDSAction.CheckingRDSStatusAsync(dbIdentifier);
                                //RDSAction.CreateDatabase(host, tenantId);
                                //RDSAction.CreateTables(host, tenantId);
                            }
                            else
                            {
                                await RDSAction.CreateNewShardAsync(dbIdentifier);
                                host = await RDSAction.CheckingRDSStatusAsync(dbIdentifier);
                                //RDSAction.CreateDatabase(host, tenantId);
                                //RDSAction.CreateTables(host, tenantId);
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
                                await RDSAction.CreateNewShardAsync(dbIdentifier);
                                host = await RDSAction.CheckingRDSStatusAsync(dbIdentifier);
                                //RDSAction.CreateDatabase(host, tenantId);
                                //RDSAction.CreateTables(host, tenantId);
                            }
                            else // Else, returning the first available RDS Cluster in the list
                            {
                                string dbIdentifier = availableIdentifier[0];
                                var sumubDomainToDbIdentifier = _tenantRepository.SumSubDomainToDbIdentifier(tenantId, dbIdentifier);
                                if (sumubDomainToDbIdentifier <= 3)
                                {
                                    host = await RDSAction.CheckingRDSStatusAsync(dbIdentifier);

                                    // Provisioning Database and tables for new tenant
                                    //RDSAction.CreateDatabase(host, tenantId);
                                    //RDSAction.CreateTables(host, tenantId);
                                }
                                else
                                {
                                    string dbIdentifierNew = $"develop-smartkarte-postgres-{rString}";
                                    await RDSAction.CreateNewShardAsync(dbIdentifierNew);
                                    host = await RDSAction.CheckingRDSStatusAsync(dbIdentifierNew);
                                    //RDSAction.CreateDatabase(host, tenantId);
                                    //RDSAction.CreateTables(host, tenantId);
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
                { "tenant_url", tenantUrl },
                { "rds_endpoint", host },
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
