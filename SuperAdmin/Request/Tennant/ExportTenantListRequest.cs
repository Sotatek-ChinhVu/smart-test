using Helper.Enum;

namespace SuperAdminAPI.Request.Tennant;

public class ExportTenantListRequest : GetTenantRequest
{
    public List<TenantEnum> ColumnView { get; set; } = new();
}
