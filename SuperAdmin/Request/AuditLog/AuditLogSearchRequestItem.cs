namespace SuperAdminAPI.Request.AuditLog;

public class AuditLogSearchRequestItem
{
    public int LogId { get; set; }

    public int TenantId { get; set; }

    public int StartDate { get; set; }

    public int EndDate { get; set; }

    public string Domain { get; set; } = string.Empty;

    public string ThreadId { get; set; } = string.Empty;

    public string LogType { get; set; } = string.Empty;

    public int HpId { get; set; }

    public int UserId { get; set; }

    public string LoginKey { get; set; } = string.Empty;

    public int DepartmentId { get; set; }

    public int SinDay { get; set; }

    public string EventCd { get; set; } = string.Empty;

    public long PtId { get; set; }

    public long RaiinNo { get; set; }

    public string Path { get; set; } = string.Empty;

    public string RequestInfo { get; set; } = string.Empty;

    public string ClientIP { get; set; } = string.Empty;

    public string Desciption { get; set; } = string.Empty;
}
