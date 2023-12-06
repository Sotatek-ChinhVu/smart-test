using Helper.Enum;

namespace SuperAdminAPI.Request.Tennant;

public class SearchTenantRequestItem
{
    public string KeyWord { get; set; } = string.Empty;

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public int Type { get; set; }

    public int StatusTenant { get; set; }

    public List<StorageFullEnum> StorageFull { get; set; } = new();
}
