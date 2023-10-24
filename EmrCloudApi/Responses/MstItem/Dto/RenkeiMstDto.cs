using Domain.Models.MstItem;

namespace EmrCloudApi.Responses.MstItem.Dto;

public class RenkeiMstDto
{
    public RenkeiMstDto(RenkeiMstModel model)
    {
        RenkeiId = model.RenkeiId;
        RenkeiName = model.RenkeiName;
        RenkeiSbt = model.RenkeiSbt;
        FunctionType = model.FunctionType;
        IsInvalid = model.IsInvalid;
        SortNo = model.SortNo;
    }

    public int RenkeiId { get; private set; }

    public string RenkeiName { get; private set; }

    public int RenkeiSbt { get; private set; }

    public int FunctionType { get; private set; }

    public int IsInvalid { get; private set; }

    public int SortNo { get; private set; }
}
