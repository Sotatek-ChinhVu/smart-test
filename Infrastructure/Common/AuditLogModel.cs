namespace Infrastructure.Common;

public class AuditLogModel
{
    public AuditLogModel(string eventCd, long ptId, int sinDay, long raiinNo, string path, string requestInfo, string description, string logType)
    {
        EventCd = eventCd;
        PtId = ptId;
        SinDay = sinDay;
        RaiinNo = raiinNo;
        Path = path;
        RequestInfo = requestInfo;
        Description = description;
        LogType = logType;
    }

    public string EventCd { get; private set; }

    public long PtId { get; private set; }

    public int SinDay { get; private set; }

    public long RaiinNo { get; private set; }

    public string Path { get; private set; }

    public string RequestInfo { get; private set; }

    public string Description { get; private set; }

    public string LogType { get; private set; }
}
