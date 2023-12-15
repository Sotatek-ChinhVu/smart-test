using SuperAdminAPI.Reponse.Tenant.Dto;

namespace SuperAdminAPI.Reponse.Tenant;

public class GetTenantResponse
{
    public GetTenantResponse(List<TenantDto> tenantList, int totalTenant)
    {
        TenantList = tenantList;
        TotalTenant = totalTenant;
    }

    public int TotalTenant { get;private set; }

    public List<TenantDto> TenantList { get;private set; }
}
