namespace Domain.Models.DrugInfor;

public class SinrekiFilterMstDetailModel
{
    public SinrekiFilterMstDetailModel(int grpCd, string itemCd, int sortNo, bool isExclude)
    {
        GrpCd = grpCd;
        ItemCd = itemCd;
        SortNo = sortNo;
        IsExclude = isExclude;
    }

    public int GrpCd { get; private set; }

    public string ItemCd { get; private set; }

    public int SortNo { get; private set; }

    public bool IsExclude { get; private set; }
}
