using Domain.SuperAdminModels.Tenant;
using Entity.SuperAdmin;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.SuperAdminRepositories
{
    public class TenantRepository : SuperAdminRepositoryBase, ITenantRepository
    {
        public TenantRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public TenantModel Get(int tenantId)
        {
            var tenant = NoTrackingDataContext.Tenants.Where(t => t.TenantId == tenantId).FirstOrDefault();
            var tenantModel = tenant == null ? new() : ConvertEntityToModel(tenant);
            return tenantModel;
        }

        public int SumSubDomainToDbIdentifier(string subDomain, string dbIdentifier)
        {
            var tenant = NoTrackingDataContext.Tenants.Where(t => t.SubDomain == subDomain && t.RdsIdentifier == dbIdentifier && t.IsDeleted == 0);
            if (tenant != null)
            {
                return tenant.Count();
            }
            return 0;
        }

        private TenantModel ConvertEntityToModel(Tenant tenant)
        {
            return new TenantModel(
                tenant.TenantId,
                tenant.Hospital,
                tenant.Status,
                tenant.AdminId,
                tenant.SubDomain,
                tenant.Db,
                tenant.Type,
                tenant.EndPointDb,
                tenant.EndSubDomain,
                tenant.Action,
                tenant.RdsIdentifier
                );
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
