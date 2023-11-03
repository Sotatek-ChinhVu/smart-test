namespace Domain.Models.DrugInfor;

public class SinrekiFilterMstDetailModel
{
    public SinrekiFilterMstDetailModel(int grpCd, string itemCd, string itemName, int sortNo, bool isExclude)
    {
        GrpCd = grpCd;
        ItemCd = itemCd;
        ItemName = itemName;
        SortNo = sortNo;
        IsExclude = isExclude;
    }

    public int GrpCd { get; private set; }

    public string ItemCd { get; private set; }

    public string ItemName { get; private set; }

    public int SortNo { get; private set; }

    public bool IsExclude { get; private set; }
}
