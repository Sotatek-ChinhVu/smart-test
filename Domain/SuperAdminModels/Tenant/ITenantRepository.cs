namespace Domain.SuperAdminModels.Tenant
{
    public interface ITenantRepository
    {
        TenantModel Get(int tenantId);
        int SumSubDomainToDbIdentifier(string subDomain, string dbIdentifier);
        bool UpgradePremium(int tenantId, string dbIdentifier, string endPoint);

        void ReleaseResource();
    }
}
