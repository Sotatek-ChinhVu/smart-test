using Domain.SuperAdminModels.Tenant;
using Helper.Enum;
using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.ExportCsvTenantList;

public class ExportCsvTenantListInputData : IInputData<ExportCsvTenantListOutputData>
{
    public ExportCsvTenantListInputData(List<TenantEnum> columnView, SearchTenantModel searchModel, Dictionary<TenantEnum, int> sortDictionary, int skip, int take)
    {
        ColumnView = columnView;
        SearchModel = searchModel;
        SortDictionary = sortDictionary;
        Skip = skip;
        Take = take;
    }

    public List<TenantEnum> ColumnView { get; private set; }

    public SearchTenantModel SearchModel { get; private set; }

    public Dictionary<TenantEnum, int> SortDictionary { get; private set; }

    public int Skip { get; private set; }

    public int Take { get; private set; }
}
