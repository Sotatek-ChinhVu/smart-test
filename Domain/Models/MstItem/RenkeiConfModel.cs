namespace Domain.Models.MstItem;

public class RenkeiConfModel
{
    public RenkeiConfModel(int renkeiId, string renkeiMstName, int seqNo, string param, int ptNumLength, int templateId, int isInvalid, string biko, int sortNo, bool isDeleted, RenkeiTemplateMstModel renkeiTemplateMstModel, RenkeiMstModel renkeiMstModel, List<RenkeiPathConfModel> renkeiPathConfModelList, List<RenkeiTimingModel> renkeiTimingModelList)
    {
        RenkeiId = renkeiId;
        RenkeiMstName = renkeiMstName;
        SeqNo = seqNo;
        Param = param;
        PtNumLength = ptNumLength;
        TemplateId = templateId;
        IsInvalid = isInvalid;
        Biko = biko;
        SortNo = sortNo;
        IsDeleted = isDeleted;
        RenkeiTemplateMstModel = renkeiTemplateMstModel;
        RenkeiMstModel = renkeiMstModel;
        RenkeiPathConfModelList = renkeiPathConfModelList;
        RenkeiTimingModelList = renkeiTimingModelList;
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

    public RenkeiTemplateMstModel RenkeiTemplateMstModel { get; private set; }

    public RenkeiMstModel RenkeiMstModel { get; private set; }

    public List<RenkeiPathConfModel> RenkeiPathConfModelList { get; private set; }

    public List<RenkeiTimingModel> RenkeiTimingModelList { get; private set; }
}
