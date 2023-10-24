using Domain.Models.MstItem;

namespace EmrCloudApi.Responses.MstItem.Dto;

public class RenkeiConfDto
{
    public RenkeiConfDto(RenkeiConfModel model)
    {
        Id = model.Id;
        RenkeiId = model.RenkeiId;
        RenkeiMstName = model.RenkeiMstName;
        SeqNo = model.SeqNo;
        Param = model.Param;
        PtNumLength = model.PtNumLength;
        TemplateId = model.TemplateId;
        IsInvalid = model.IsInvalid;
        Biko = model.Biko;
        SortNo = model.SortNo;
        IsDeleted = model.IsDeleted;
        RenkeiPathConfModelList = model.RenkeiPathConfModelList.Select(item => new RenkeiPathConfDto(item)).ToList();
        RenkeiTimingModelList = model.RenkeiTimingModelList.Select(item => new RenkeiTimingDto(item)).ToList();
    }

    public long Id { get; private set; }

    public int RenkeiId { get; private set; }

    public string RenkeiMstName { get; private set; }

    public int SeqNo { get; private set; }

    public string Param { get; private set; }

    public int PtNumLength { get; private set; }

    public int TemplateId { get; private set; }

    public int IsInvalid { get; private set; }

    public string Biko { get; private set; }

    public int SortNo { get; private set; }

    public bool IsDeleted { get; private set; }

    public List<RenkeiPathConfDto> RenkeiPathConfModelList { get; private set; }

    public List<RenkeiTimingDto> RenkeiTimingModelList { get; private set; }
}
