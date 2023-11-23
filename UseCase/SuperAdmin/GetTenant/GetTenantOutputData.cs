using Domain.SuperAdminModels.Tenant;
using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.GetTenant;

public class GetTenantOutputData : IOutputData
{
    public GetTenantOutputData(List<TenantModel> tenantList, GetTenantStatus status)
    {
        TenantList = tenantList;
        Status = status;
    }

    public List<TenantModel> TenantList { get; private set; }

    public GetTenantStatus Status { get; private set; }
}
