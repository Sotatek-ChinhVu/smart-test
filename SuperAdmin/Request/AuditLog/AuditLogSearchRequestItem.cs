namespace SuperAdminAPI.Request.AuditLog;

public class AuditLogSearchRequestItem
{
    public int LogId { get; set; } = 0;

    public DateTime? StartDate { get; set; } = null;

    public DateTime? EndDate { get; set; } = null;

    public string Domain { get; set; } = string.Empty;

    public string ThreadId { get; set; } = string.Empty;

    public string LogType { get; set; } = string.Empty;

    public int HpId { get; set; } = 0;

    public int UserId { get; set; } = 0;

    public string LoginKey { get; set; } = string.Empty;

    public int DepartmentId { get; set; } = 0;

    public int SinDay { get; set; } = 0;

    public string EventCd { get; set; } = string.Empty;

    public long PtId { get; set; } = 0;

    public long RaiinNo { get; set; } = 0;

    public string Path { get; set; } = string.Empty;

    public string RequestInfo { get; set; } = string.Empty;

    public string ClientIP { get; set; } = string.Empty;

    public string Desciption { get; set; } = string.Empty;
}
