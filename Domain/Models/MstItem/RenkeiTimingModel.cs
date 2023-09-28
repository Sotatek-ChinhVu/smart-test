namespace Domain.Models.MstItem;

public class RenkeiTimingModel
{
    public RenkeiTimingModel(long id, string eventName, int renkeiId, int seqNo, string eventCd, int isInvalid, bool isDeleted)
    {
        Id = id;
        EventName = eventName;
        RenkeiId = renkeiId;
        SeqNo = seqNo;
        EventCd = eventCd;
        IsInvalid = isInvalid;
        IsDeleted = isDeleted;
    }

    public long Id { get; private set; }

    public string EventName { get; private set; }

    public int RenkeiId { get; private set; }

    public int SeqNo { get; private set; }

    public string EventCd { get; private set; }

    public int IsInvalid { get; private set; }

    public bool IsDeleted { get; private set; }
}
