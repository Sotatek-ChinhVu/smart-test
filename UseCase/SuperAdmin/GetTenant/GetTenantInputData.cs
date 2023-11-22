using Domain.SuperAdminModels.Tenant;
using Helper.Enum;
using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.GetTenant;

public class GetTenantInputData : IInputData<GetTenantOutputData>
{
    public GetTenantInputData(int tenantId, SearchTenantModel searchModel, Dictionary<TenantEnum, int> sortDictionary, int skip, int take)
    {
        TenantId = tenantId;
        SearchModel = searchModel;
        SortDictionary = sortDictionary;
        Skip = skip;
        Take = take;
    }

    public int TenantId { get; private set; }

    public SearchTenantModel SearchModel { get; private set; }

    public Dictionary<TenantEnum, int> SortDictionary { get; private set; }

    public int Skip { get; private set; }

    public int Take { get; private set; }
}
