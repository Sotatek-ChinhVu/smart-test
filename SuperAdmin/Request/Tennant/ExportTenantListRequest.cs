using Helper.Enum;

namespace SuperAdminAPI.Request.Tennant;

public class ExportTenantListRequest
{
    public SearchTenantRequestItem SearchModel { get; set; } = new();

    public Dictionary<TenantEnum, int> SortDictionary { get; set; } = new();

    public List<TenantEnum> ColumnView { get; set; } = new();
}
