namespace Domain.SuperAdminModels.Logger;

public class AuditLogSearchModel
{
    public AuditLogSearchModel(int logId, DateTime? startDate, DateTime? endDate, string domain, string threadId, string logType, int hpId, int userId, string loginKey, int departmentId, int sinDay, string eventCd, long ptId, long raiinNo, string path, string requestInfo, string clientIP, string desciption)
    {
        LogId = logId;
        StartDate = startDate;
        EndDate = endDate;
        Domain = domain;
        ThreadId = threadId;
        LogType = logType;
        HpId = hpId;
        UserId = userId;
        LoginKey = loginKey;
        DepartmentId = departmentId;
        SinDay = sinDay;
        EventCd = eventCd;
        PtId = ptId;
        RaiinNo = raiinNo;
        Path = path;
        RequestInfo = requestInfo;
        ClientIP = clientIP;
        Desciption = desciption;
    }

    public int LogId { get; private set; }

    public DateTime? StartDate { get; private set; }

    public DateTime? EndDate { get; private set; }

    public string Domain { get; private set; }

    public string ThreadId { get; private set; }

    public string LogType { get; private set; }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public string LoginKey { get; private set; }

    public int DepartmentId { get; private set; }

    public int SinDay { get; private set; }

    public string EventCd { get; private set; }

    public long PtId { get; private set; }

    public long RaiinNo { get; private set; }

    public string Path { get; private set; }

    public string RequestInfo { get; private set; }

    public string ClientIP { get; private set; }

    public string Desciption { get; private set; }

    public bool IsEmptyModel
    {
        get
        {
            return string.IsNullOrEmpty(Domain)
                   && string.IsNullOrEmpty(ThreadId)
                   && string.IsNullOrEmpty(LogType)
                   && HpId == 0
                   && StartDate == null
                   && EndDate == null
                   && UserId == 0
                   && string.IsNullOrEmpty(LoginKey)
                   && DepartmentId == 0
                   && SinDay == 0
                   && string.IsNullOrEmpty(EventCd)
                   && PtId == 0
                   && RaiinNo == 0
                   && string.IsNullOrEmpty(Path)
                   && string.IsNullOrEmpty(RequestInfo)
                   && string.IsNullOrEmpty(ClientIP)
                   && string.IsNullOrEmpty(Desciption);
        }
    }
}
