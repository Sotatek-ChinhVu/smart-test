namespace Domain.Models.AuditTrailLog;

public class AuditTraiLogModel
{
    public AuditTraiLogModel(int hpId, int userId, string eventCd, long ptId, int sinDay, long raiinNo, string hosoku)
    {
        HpId = hpId;
        UserId = userId;
        EventCd = eventCd;
        PtId = ptId;
        SinDay = sinDay;
        RaiinNo = raiinNo;
        Hosoku = hosoku;
    }

    public int HpId { get; private set; }
    
    public int UserId { get; private set; }
    
    public string EventCd { get; private set; }
    
    public long PtId { get; private set; }
    
    public int SinDay { get; private set; }
    
    public long RaiinNo { get; private set; }
    
    public string Hosoku { get; private set; }
}
