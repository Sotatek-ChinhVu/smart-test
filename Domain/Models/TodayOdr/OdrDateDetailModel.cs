namespace Domain.Models.TodayOdr;

public class OdrDateDetailModel
{
    public OdrDateDetailModel(int grpId, int seqNo, string itemCd, string itemName, int sortNo)
    {
        GrpId = grpId;
        SeqNo = seqNo;
        ItemCd = itemCd;
        ItemName = itemName;
        SortNo = sortNo;
    }

    public OdrDateDetailModel(int grpId, int seqNo, string itemCd, string itemName, int sortNo, bool isDeleted)
    {
        GrpId = grpId;
        SeqNo = seqNo;
        ItemCd = itemCd;
        ItemName = itemName;
        SortNo = sortNo;
        IsDeleted = isDeleted;
    }

    public int GrpId { get; private set; }

    public int SeqNo { get; private set; }

    public string ItemCd { get; private set; }

    public string ItemName { get; private set; }

    public int SortNo { get; private set; }

    public bool IsDeleted { get; private set; }
}
