using SuperAdminAPI.Reponse.Tenant.Dto;

namespace SuperAdminAPI.Reponse.Tenant;

public class GetTenantResponse
{
    public GetTenantResponse(List<TenantDto> tenantList)
    {
        TenantList = tenantList;
    }

    public List<TenantDto> TenantList { get;private set; }
}
