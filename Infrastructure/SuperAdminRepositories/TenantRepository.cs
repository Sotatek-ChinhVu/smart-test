using Domain.SuperAdminModels.Tenant;
using Entity.SuperAdmin;
using Helper.Common;
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

        public int GetBySubDomainAndIdentifier(string subDomain, string Identifier)
        {
            var tenant = NoTrackingDataContext.Tenants.Where(t => t.SubDomain == subDomain && t.RdsIdentifier == Identifier && t.IsDeleted == 0).FirstOrDefault();
            return tenant == null ? 0 : tenant.TenantId;
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
                tenant.Password,
                tenant.SubDomain,
                tenant.Db,
                tenant.Size,
                tenant.Type,
                tenant.EndPointDb,
                tenant.EndSubDomain,
                tenant.Action,
                tenant.RdsIdentifier
                );
        }

        public int CreateTenant(TenantModel model)
        {
            var tenant = new Tenant();
            tenant.Hospital = model.Hospital;
            tenant.AdminId = model.AdminId;
            tenant.Password = model.Password;
            tenant.SubDomain = model.SubDomain;

            tenant.Status = 2; //Status: creating
            tenant.Db = model.Db;
            tenant.Size = model.Size;
            tenant.Type = model.Type;
            tenant.EndPointDb = model.SubDomain;
            tenant.EndSubDomain = model.SubDomain;
            tenant.RdsIdentifier = model.RdsIdentifier;
            tenant.IsDeleted = 0;
            tenant.CreateDate = CIUtil.GetJapanDateTimeNow();
            tenant.UpdateDate = CIUtil.GetJapanDateTimeNow();
            TrackingDataContext.Tenants.Add(tenant);
            TrackingDataContext.SaveChanges();
            return tenant.TenantId;
        }
        public bool UpdateStatusTenant(int tenantId, byte status, string endSubDomain, string endPointDb, string dbIdentifier)
        {
            var tenant = TrackingDataContext.Tenants.FirstOrDefault(i => i.TenantId == tenantId);
            if (tenant != null)
            {
                if (!string.IsNullOrEmpty(endPointDb))
                {
                    tenant.EndPointDb = endPointDb;
                }
                if (!string.IsNullOrEmpty(endSubDomain))
                {
                    tenant.SubDomain = endSubDomain;
                }
                if (!string.IsNullOrEmpty(dbIdentifier))
                {
                    tenant.RdsIdentifier = dbIdentifier;
                }
                tenant.Status = status;
                tenant.UpdateDate = CIUtil.GetJapanDateTimeNow();
                tenant.CreateDate = TimeZoneInfo.ConvertTimeToUtc(tenant.CreateDate);
                TrackingDataContext.Tenants.Update(tenant);
            }
            return TrackingDataContext.SaveChanges() > 0;
        }
        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
