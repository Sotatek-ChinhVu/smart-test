using Helper.Enum;

namespace Domain.SuperAdminModels.Tenant;

public interface ITenantRepository
{
    TenantModel Get(int tenantId);

    int GetBySubDomainAndIdentifier(string subDomain, string Identifier);

    int SumSubDomainToDbIdentifier(string dbIdentifier);

    int CreateTenant(TenantModel model);

    bool UpdateInfTenant(int tenantId, byte status, string endSubDomain, string endPointDb, string dbIdentifier);

    bool UpdateStatusTenant(int tenantId, byte status);

    TenantModel UpgradePremium(int tenantId, string dbIdentifier, string endPoint, string subDomain, int size, int sizeType);

    TenantModel TerminateTenant(int tenantId, byte TerminateStatus);

    List<TenantModel> GetTenantList(SearchTenantModel searchModel, Dictionary<TenantEnum, int> sortDictionary, int skip, int take);

    void RevokeInsertPermission();

    TenantModel GetTenant(int tenantId);

    void ReleaseResource();
}
