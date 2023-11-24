using Domain.SuperAdminModels.Tenant;
using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.GetTenantDetail;

public class GetTenantDetailOutputData : IOutputData
{
    public GetTenantDetailOutputData(TenantModel tenant, GetTenantDetailStatus status)
    {
        Tenant = tenant;
        Status = status;
    }

    public TenantModel Tenant { get; private set; }

    public GetTenantDetailStatus Status { get; private set; }
}
