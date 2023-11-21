namespace Domain.SuperAdminModels.Tenant
{
    public interface ITenantRepository
    {
        TenantModel Get(int tenantId);
        int GetBySubDomainAndIdentifier(string subDomain, string Identifier);
        int SumSubDomainToDbIdentifier(string subDomain, string dbIdentifier);
        int CreateTenant(TenantModel model);
        bool UpdateInfTenant(int tenantId, byte status, string endSubDomain, string endPoint, string dbIdentifier);
        bool UpdateStatusTenant(int tenantId, byte status);
        TenantModel UpgradePremium(int tenantId, string dbIdentifier, string endPoint);
        void ReleaseResource();
    }
}
