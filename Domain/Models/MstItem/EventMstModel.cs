namespace Domain.Models.MstItem;

public class EventMstModel
{
    public EventMstModel(string eventCd, string eventName, int auditTrailing)
    {
        EventCd = eventCd;
        EventName = eventName;
        AuditTrailing = auditTrailing;
    }

    public string EventCd { get; private set; }

    public string EventName { get; private set; }

    public int AuditTrailing { get; private set; }
}
