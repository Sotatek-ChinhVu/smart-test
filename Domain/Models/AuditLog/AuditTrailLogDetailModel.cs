using System.Text.Json.Serialization;

namespace Domain.Models.AuditLog
{
    public class AuditTrailLogDetailModel
    {

        public AuditTrailLogDetailModel()
        {
            Hosoku = string.Empty;
        }
        [JsonConstructor]
        public AuditTrailLogDetailModel(long logId, string hosoku)
        {
            LogId = logId;
            Hosoku = hosoku;
        }

        public long LogId { get; private set; }

        public string Hosoku { get; private set; }
    }
}
