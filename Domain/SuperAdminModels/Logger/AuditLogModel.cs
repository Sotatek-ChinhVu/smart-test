using Helper.Common;

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

    public long LogId { get; private set; }

    public int TenantId { get; private set; }

    public string Domain { get; private set; }

    public string ThreadId { get; private set; }

    public string LogType { get; private set; }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public string LoginKey { get; private set; }

    public int DepartmentId { get; private set; }

    public DateTime LogDate { get; private set; }

    public string EventCd { get; private set; }

    public long PtId { get; private set; }

    public int SinDay { get; private set; }

    public long RaiinNo { get; private set; }

    public string Path { get; private set; }

    public string RequestInfo { get; private set; }

    public string ClientIP { get; private set; }

    public string Desciption { get; private set; }
}
