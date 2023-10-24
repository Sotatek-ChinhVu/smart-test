using Domain.Models.Online;

namespace EmrCloudApi.Responses.Online.Dto;

public class OnlineConsentDto
{
    public OnlineConsentDto(OnlineConsentModel model)
    {
        PtId = model.PtId;
        ConsKbn = model.ConsKbn;
        ConsDate = model.ConsDate;
        LimitDate = model.LimitDate;
    }

    public long PtId { get; private set; }

    public int ConsKbn { get; private set; }

    public DateTime ConsDate { get; private set; }

    public DateTime LimitDate { get; private set; }
}
