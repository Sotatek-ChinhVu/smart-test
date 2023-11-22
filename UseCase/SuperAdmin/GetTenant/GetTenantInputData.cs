using Domain.SuperAdminModels.Tenant;
using Helper.Enum;
using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.GetTenant;

public class GetTenantInputData : IInputData<GetTenantOutputData>
{
    public GetTenantInputData(SearchTenantModel searchModel, Dictionary<TenantEnum, int> sortDictionary, int skip, int take)
    {
        SearchModel = searchModel;
        SortDictionary = sortDictionary;
        Skip = skip;
        Take = take;
    }

    public SearchTenantModel SearchModel { get; private set; }

    public Dictionary<TenantEnum, int> SortDictionary { get; private set; }

    public int Skip { get; private set; }

    public int Take { get; private set; }
}
