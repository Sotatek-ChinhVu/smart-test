using System.Text.Json.Serialization;

namespace Domain.Models.AuditLog
{
    public class AuditTrailLogModel
    {
        public AuditTrailLogModel(long logId, DateTime logDate, int hpId, int userId, string eventCd, long ptId, int sinDate, long raiinNo, string machine, string hosuke)
        {
            LogId = logId;
            LogDate = logDate;
            HpId = hpId;
            UserId = userId;
            EventCd = eventCd;
            PtId = ptId;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            Machine = machine;
            Hosuke = hosuke;
            AuditTrailLogDetailModel = new();
        }

        [JsonConstructor]
        public AuditTrailLogModel(long logId, DateTime logDate, int hpId, int userId, string eventCd, long ptId, int sinDate, long raiinNo, string machine, string hosuke, AuditTrailLogDetailModel auditTrailLogDetailModel)
        {
            LogId = logId;
            LogDate = logDate;
            HpId = hpId;
            UserId = userId;
            EventCd = eventCd;
            PtId = ptId;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            Machine = machine;
            Hosuke = hosuke;
            AuditTrailLogDetailModel = auditTrailLogDetailModel;
        }

        public AuditTrailLogModel()
        {
            EventCd = string.Empty;
            Machine = string.Empty;
            Hosuke = string.Empty;
            AuditTrailLogDetailModel = new();
        }

        public long LogId { get; private set; }

        public DateTime LogDate { get; private set; }

        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public string EventCd { get; private set; }

        public long PtId { get; private set; }

        public int SinDate { get; private set; }

        public long RaiinNo { get; private set; }

        public string Machine { get; private set; }

        public string Hosuke { get; private set; }

        public AuditTrailLogDetailModel AuditTrailLogDetailModel { get; private set; }
    }
}
