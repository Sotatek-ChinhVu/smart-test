using Domain.Models.TodayOdr;

namespace EmrCloudApi.Responses.LastDayInformation.Dto;

public class OdrDateDetailDto
{
    public OdrDateDetailDto(OdrDateDetailModel model)
    {
        SeqNo = model.SeqNo;
        ItemCd = model.ItemCd;
        ItemName = model.ItemName;
        SortNo = model.SortNo;
    }

    public int SeqNo { get; private set; }

    public string ItemCd { get; private set; }

    public string ItemName { get; private set; }

    public int SortNo { get; private set; }
}
