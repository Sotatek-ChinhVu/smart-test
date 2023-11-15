using AWSSDK.Common;
using AWSSDK.Constants;
using AWSSDK.Interfaces;

namespace AWSSDK.Services
{
    public class AwsSdkService : IAwsSdkService
    {
        public AwsSdkService() { }
        public async Task<Dictionary<string, Dictionary<string, string>>> SummaryCard()
        {
            return await CloudWatchAction.GetSummaryCardAsync();
        }
        public async Task<List<string>> GetAvailableIdentifiersAsync()
        {
            var sumaryCard = await CloudWatchAction.GetSummaryCardAsync();
            var result = sumaryCard.Where(entry => entry.Value["available"] == "yes").Select(entry => entry.Key).ToList();
            return result;
        }

        public async Task<Dictionary<string, string>> TenantOnboardAsync(string tenantId, string maxCcu, string tier)
        {
            string rString = CommonConstants.GenerateRandomString(6);
            string tenantUrl = "";
            string host = "";

            try
            {
                // Provisioning SubDomain for new tenants
                if (!string.IsNullOrEmpty(tenantId))
                {
                    tenantUrl = $"{tenantId}.smartkarte.org";
                    await Route53Action.CreateTenantDomain(tenantId);
                    await CloudFrontAction.UpdateNewTenantAsync(tenantId);

                    // Checking Available RDS Cluster
                    if (tenantId.Length > 0)
                    {
                        // Checking tenant tier, if dedicated, provision new RDS instance
                        if (tier == "dedicated")
                        {
                            string dbIdentifier = $"develop-smartkarte-postgres-{rString}";
                            var rdsInfo = await RDSAction.GetRDSInformation();
                            if (rdsInfo.ContainsKey(dbIdentifier))
                            {
                                host = await RDSAction.CheckingRDSStatusAsync(dbIdentifier);
                                RDSAction.CreateDatabase(host, tenantId);
                                RDSAction.CreateTables(host, tenantId);
                            }
                            else
                            {
                                await RDSAction.CreateNewShardAsync(dbIdentifier);
                                host = await RDSAction.CheckingRDSStatusAsync(dbIdentifier);
                                RDSAction.CreateDatabase(host, tenantId);
                                RDSAction.CreateTables(host, tenantId);
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
                                RDSAction.CreateDatabase(host, tenantId);
                                RDSAction.CreateTables(host, tenantId);
                            }
                            else // Else, returning the first available RDS Cluster in the list
                            {
                                string dbIdentifier = availableIdentifier[0];
                                host = await RDSAction.CheckingRDSStatusAsync(dbIdentifier);

                                // Provisioning Database and tables for new tenant
                                RDSAction.CreateDatabase(host, tenantId);
                                RDSAction.CreateTables(host, tenantId);
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
                { "max_ccu", maxCcu },
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
