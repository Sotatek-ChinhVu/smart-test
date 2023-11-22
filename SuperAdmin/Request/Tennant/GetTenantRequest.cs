using Helper.Enum;

namespace SuperAdminAPI.Request.Tennant;

public class GetTenantRequest
{
    public int TenantId { get; set; }

    public SearchTenantRequestItem SearchModel { get; set; } = new();

    public Dictionary<TenantEnum, int> SortDictionary { get; set; } = new();

    public int Skip { get; set; }

    public int Take { get; set; }
}
