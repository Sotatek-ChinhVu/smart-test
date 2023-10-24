namespace Domain.Models.MstItem;

public class RenkeiMstModel
{
    public RenkeiMstModel(int hpId, int renkeiId, string renkeiName, int renkeiSbt, int functionType, int isInvalid, int sortNo)
    {
        HpId = hpId;
        RenkeiId = renkeiId;
        RenkeiName = renkeiName;
        RenkeiSbt = renkeiSbt;
        FunctionType = functionType;
        IsInvalid = isInvalid;
        SortNo = sortNo;
    }

    public int HpId { get; private set; }

    public int RenkeiId { get; private set; }

    public string RenkeiName { get; private set; }

    public int RenkeiSbt { get; private set; }

    public int FunctionType { get; private set; }

    public int IsInvalid { get; private set; }

    public int SortNo { get; private set; }
}
