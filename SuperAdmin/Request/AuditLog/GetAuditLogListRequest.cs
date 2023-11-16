using Helper.Enum;

namespace SuperAdminAPI.Request.AuditLog;

public class GetAuditLogListRequest
{
    public int TenantId { get; set; }

    public AuditLogSearchRequestItem RequestModel { get; set; } = new();

    public Dictionary<AuditLogEnum, int> SortDictionary { get; set; } = new();

    public int Skip { get; set; }

    public int Take { get; set; }
}
