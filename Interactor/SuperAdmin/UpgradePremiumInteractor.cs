using AWSSDK.Common;
using AWSSDK.Constants;
using AWSSDK.Interfaces;
using Domain.SuperAdminModels.Tenant;
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

                // Exit domain 
                var checkSubDomain = _awsSdkService.CheckSubdomainExistenceAsync(tenant.SubDomain).Result;
                if (checkSubDomain)
                {
                    return new UpgradePremiumOutputData(false, UpgradePremiumStatus.FailedTenantIsPremium);
                }



                _ = Task.Run(() =>
                {

                    var createSnapshot = _awsSdkService.CreateDBSnapshotAsync(tenant.RdsIdentifier);
                    // Create RSD  preminum
                    var createTenantResult = TenantOnboardAsync(tenant.SubDomain, 0, 0).Result;
                    var dbInstanceIdentifier = "";
                    var snapshotIdentifier = "";

                    // Restore DB Instance from snapshot
                    _awsSdkService.RestoreDBInstanceFromSnapshot(dbInstanceIdentifier, snapshotIdentifier);
                });

                return new UpgradePremiumOutputData(true, UpgradePremiumStatus.Successed);
            }
            finally
            {
                _tenantRepository.ReleaseResource();
            }
        }

        public async Task<Dictionary<string, string>> TenantOnboardAsync(string tenantId, int size, int sizeType)
        {
            string rString = CommonConstants.GenerateRandomString(6);
            string tenantUrl = "";
            string host = "";
            string dbIdentifier = "";

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

                        dbIdentifier = $"develop-smartkarte-postgres-{rString}";
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
                { "dbIdentifier", dbIdentifier },
                { "rds_endpoint", host }
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
