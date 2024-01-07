using Helper.Enum;

namespace Domain.SuperAdminModels.Tenant;

public interface ITenantRepository
{
    TenantModel Get(int tenantId);

    TenantModel GetTenantBySubDomain(string subDomain);

    TenantModel GetByStatus(int tenantId, byte status);

    int GetBySubDomainAndIdentifier(string subDomain, string Identifier);

    int SumSubDomainToDbIdentifier(string dbIdentifier);

    int CreateTenant(TenantModel model);

    bool UpdateInfTenant(int tenantId, byte status, string endSubDomain, string endPointDb, string dbIdentifier);

    bool UpdateStatusTenant(int tenantId, byte status);

    bool UpdateTenantIsRestoreS3(int tenantId, bool isRestoredS3);

    TenantModel UpdateTenant(int tenantId, string dbIdentifier, string endPoint, string subDomain, int size, int sizeType, string hospital, int adminId, string password, string endSubDomain, byte status, byte type);

    TenantModel TerminateTenant(int tenantId, byte TerminateStatus);

    (List<TenantModel> TenantList, int TotalTenant) GetTenantList(SearchTenantModel searchModel, Dictionary<TenantEnum, int> sortDictionary, int skip, int take, bool getDataReport = false);

    void RevokeInsertPermission();

    TenantModel GetTenant(int tenantId);

    bool CheckExistsHospital(string hospital);

    bool CheckExistsSubDomain(string subDomain);

    bool UpdateInfTenantStatus(int tenantId, byte status);

    List<TenantModel> GetByRdsId(int tenantId, string rdsIdentifier);

    List<TenantModel> GetTenantByStatus(List<byte> status);

    void ReleaseResource();

}
