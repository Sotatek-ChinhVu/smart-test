using Helper.Enum;

namespace SuperAdminAPI.Request.AuditLog;

public class ExportAuditLogListRequest
{
    public int TenantId { get; set; }

    public AuditLogSearchRequestItem RequestModel { get; set; } = new();

    public Dictionary<AuditLogEnum, int> SortDictionary { get; set; } = new();

    public List<AuditLogEnum> ColumnView { get; set; } = new();
}
