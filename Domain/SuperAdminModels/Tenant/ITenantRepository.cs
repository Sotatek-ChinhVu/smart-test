using Helper.Enum;

namespace Domain.SuperAdminModels.Tenant;

public interface ITenantRepository
{
    TenantModel Get(int tenantId);

    TenantModel GetByStatus(int tenantId, byte status);

    int GetBySubDomainAndIdentifier(string subDomain, string Identifier);

    int SumSubDomainToDbIdentifier(string dbIdentifier);

    int CreateTenant(TenantModel model);

    bool UpdateInfTenant(int tenantId, byte status, string endSubDomain, string endPointDb, string dbIdentifier);

    bool UpdateStatusTenant(int tenantId, byte status);

    TenantModel UpdateTenant(int tenantId, string dbIdentifier, string endPoint, string subDomain, int size, int sizeType, string hospital, int adminId, string password, string endSubDomain, byte status);

    TenantModel TerminateTenant(int tenantId, byte TerminateStatus);

    (List<TenantModel> TenantList, int TotalTenant) GetTenantList(SearchTenantModel searchModel, Dictionary<TenantEnum, int> sortDictionary, int skip, int take);

    void RevokeInsertPermission();

    TenantModel GetTenant(int tenantId);
   
    bool CheckExistsHospital(string hospital);

    bool CheckExistsSubDomain(string subDomain);

    bool UpdateInfTenantStatus(int tenantId, byte status);

    void ReleaseResource();

}
