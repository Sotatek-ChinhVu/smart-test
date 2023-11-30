using SuperAdminAPI.Reponse.Tenant.Dto;

namespace SuperAdminAPI.Reponse.Tenant;

public class GetTenantDetailResponse
{
    public GetTenantDetailResponse(TenantDto tenant)
    {
        Tenant = tenant;
    }

    public TenantDto Tenant { get; private set; }
}
