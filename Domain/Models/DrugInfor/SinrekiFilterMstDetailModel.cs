namespace Domain.Models.DrugInfor;

public class SinrekiFilterMstDetailModel
{
    public SinrekiFilterMstDetailModel(long id, int grpCd, string itemCd, string itemName, int sortNo, bool isExclude, bool isDeleted)
    {
        Id = id;
        GrpCd = grpCd;
        ItemCd = itemCd;
        ItemName = itemName;
        SortNo = sortNo;
        IsExclude = isExclude;
        IsDeleted = isDeleted;
    }

    public SinrekiFilterMstDetailModel(long id, int grpCd, string itemCd, string itemName, int sortNo, bool isExclude)
    {
        Id = id;
        GrpCd = grpCd;
        ItemCd = itemCd;
        ItemName = itemName;
        SortNo = sortNo;
        IsExclude = isExclude;
        IsDeleted = false;
    }

    public long Id { get; private set; }

    public int GrpCd { get; private set; }

    public string ItemCd { get; private set; }

    public string ItemName { get; private set; }

    public int SortNo { get; private set; }

    public bool IsExclude { get; private set; }

    public bool IsDeleted { get; private set; }
}
