namespace Domain.Models.AuditLog
{
    public class EventMstModel
    {
        public EventMstModel(string eventCd, string eventName, int auditTrailing, DateTime createDate)
        {
            EventCd = eventCd;
            EventName = eventName;
            AuditTrailing = auditTrailing;
            CreateDate = createDate;
        }

        public string EventCd { get; private set; }

        public string EventName { get; private set; }

        public int AuditTrailing { get; private set; }

        public DateTime CreateDate { get; private set; }
    }
}
