namespace Domain.Models.Online;

public class OnlineConsentModel
{
    public OnlineConsentModel(long ptId, int consKbn, DateTime consDate, DateTime limitDate)
    {
        PtId = ptId;
        ConsKbn = consKbn;
        ConsDate = consDate;
        LimitDate = limitDate;
    }

    public long PtId { get; private set; }

    public int ConsKbn { get; private set; }

    public DateTime ConsDate { get; private set; }

    public DateTime LimitDate { get; private set; }
}
