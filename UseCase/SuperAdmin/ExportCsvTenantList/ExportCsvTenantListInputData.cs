using Domain.SuperAdminModels.Tenant;
using Helper.Enum;
using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.ExportCsvTenantList;

public class ExportCsvTenantListInputData : IInputData<ExportCsvTenantListOutputData>
{
    public ExportCsvTenantListInputData(List<TenantEnum> columnView, SearchTenantModel searchModel, Dictionary<TenantEnum, int> sortDictionary)
    {
        ColumnView = columnView;
        SearchModel = searchModel;
        SortDictionary = sortDictionary;
    }

    public List<TenantEnum> ColumnView { get; private set; }

    public SearchTenantModel SearchModel { get; private set; }

    public Dictionary<TenantEnum, int> SortDictionary { get; private set; }
}
