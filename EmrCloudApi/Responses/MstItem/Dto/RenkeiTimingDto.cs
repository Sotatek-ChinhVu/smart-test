using Domain.Models.MstItem;

namespace EmrCloudApi.Responses.MstItem.Dto;

public class RenkeiTimingDto
{
    public RenkeiTimingDto(RenkeiTimingModel model)
    {
        Id = model.Id;
        EventName = model.EventName;
        RenkeiId = model.RenkeiId;
        SeqNo = model.SeqNo;
        EventCd = model.EventCd;
        IsInvalid = model.IsInvalid;
        IsDeleted = model.IsDeleted;
    }

    public long Id { get; private set; }

    public string EventName { get; private set; }

    public int RenkeiId { get; private set; }

    public int SeqNo { get; private set; }

    public string EventCd { get; private set; }

    public int IsInvalid { get; private set; }

    public bool IsDeleted { get; private set; }
}
