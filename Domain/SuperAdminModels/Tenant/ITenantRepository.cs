namespace Domain.SuperAdminModels.Tenant;

public interface ITenantRepository
{
    TenantModel Get(int tenantId);

    int SumSubDomainToDbIdentifier(string subDomain, string dbIdentifier);

    void ReleaseResource();
}
