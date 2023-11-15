namespace Domain.SuperAdminModels.Logger;

public class AuditLogModel
{
    public AuditLogModel(long logId, int tenantId, string domain, string threadId, string logType, int hpId, int userId, string loginKey, int departmentId, DateTime logDate, string eventCd, long ptId, int sinDay, long raiinNo, string path, string requestInfo, string clientIP, string desciption)
    {
        LogId = logId;
        TenantId = tenantId;
        Domain = domain;
        ThreadId = threadId;
        LogType = logType;
        HpId = hpId;
        UserId = userId;
        LoginKey = loginKey;
        DepartmentId = departmentId;
        LogDate = logDate;
        EventCd = eventCd;
        PtId = ptId;
        SinDay = sinDay;
        RaiinNo = raiinNo;
        Path = path;
        RequestInfo = requestInfo;
        ClientIP = clientIP;
        Desciption = desciption;
    }

    public long LogId { get; set; }

    public int TenantId { get; set; }

    public string Domain { get; set; }

    public string ThreadId { get; set; }

    public string LogType { get; set; } 

    public int HpId { get; set; }

    public int UserId { get; set; }

    public string LoginKey { get; set; } 

    public int DepartmentId { get; set; }

    public DateTime LogDate { get; set; }

    public string EventCd { get; set; } 

    public long PtId { get; set; }

    public int SinDay { get; set; }

    public long RaiinNo { get; set; }

    public string Path { get; set; } 

    public string RequestInfo { get; set; }

    public string ClientIP { get; set; }

    public string Desciption { get; set; }
}
