using Domain.Models.MstItem;

namespace EmrCloudApi.Responses.MstItem.Dto;

public class RenkeiConfDto
{
    public RenkeiConfDto(RenkeiConfModel model)
    {
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
        RenkeiTemplateMstModel = new RenkeiTemplateMstDto(model.RenkeiTemplateMstModel);
        RenkeiMstModel = new RenkeiMstDto(model.RenkeiMstModel);
        RenkeiPathConfModelList = model.RenkeiPathConfModelList.Select(item => new RenkeiPathConfDto(item)).ToList();
        RenkeiTimingModelList = model.RenkeiTimingModelList.Select(item => new RenkeiTimingDto(item)).ToList();
    }

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

    public RenkeiTemplateMstDto RenkeiTemplateMstModel { get; private set; }

    public RenkeiMstDto RenkeiMstModel { get; private set; }

    public List<RenkeiPathConfDto> RenkeiPathConfModelList { get; private set; }

    public List<RenkeiTimingDto> RenkeiTimingModelList { get; private set; }
}
