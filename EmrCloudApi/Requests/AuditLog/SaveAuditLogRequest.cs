using Domain.Models.AuditLog;
using Newtonsoft.Json;

namespace EmrCloudApi.Requests.AuditLog;

public class SaveAuditLogRequest
{
    public SaveAuditLogItem AuditTrailLogModel { get; set; } = new();

}

public class SaveAuditLogItem
{
    [JsonConstructor]
    public SaveAuditLogItem(long logId, string eventCd, long ptId, int sinDate, long raiinNo, string machine, string hosuke, AuditTrailLogDetailModel auditTrailLogDetailModel)
    {
        LogId = logId;
        EventCd = eventCd;
        PtId = ptId;
        SinDate = sinDate;
        RaiinNo = raiinNo;
        Machine = machine;
        Hosuke = hosuke;
        AuditTrailLogDetailModel = auditTrailLogDetailModel;
    }

    public SaveAuditLogItem()
    {
        EventCd = string.Empty;
        Machine = string.Empty;
        Hosuke = string.Empty;
        AuditTrailLogDetailModel = new();
    }

    public long LogId { get; set; }

    public string EventCd { get; set; }

    public long PtId { get; set; }

    public int SinDate { get; set; }

    public long RaiinNo { get; set; }

    public string Machine { get; set; }

    public string Hosuke { get; set; }

    public AuditTrailLogDetailModel AuditTrailLogDetailModel { get; set; }
}
