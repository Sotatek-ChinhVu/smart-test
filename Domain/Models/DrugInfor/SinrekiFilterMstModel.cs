namespace Domain.Models.DrugInfor;

public class SinrekiFilterMstModel
{
    public SinrekiFilterMstModel(int grpCd, string name, int sortNo, List<SinrekiFilterMstKouiModel> sinrekiFilterMstKouiList, List<SinrekiFilterMstDetailModel> sinrekiFilterMstDetailList)
    {
        GrpCd = grpCd;
        Name = name;
        SortNo = sortNo;
        SinrekiFilterMstKouiList = sinrekiFilterMstKouiList;
        SinrekiFilterMstDetailList = sinrekiFilterMstDetailList;
    }

    public int GrpCd { get; private set; }

    public string Name { get; private set; }

    public int SortNo { get; private set; }

    public List<SinrekiFilterMstKouiModel> SinrekiFilterMstKouiList { get; private set; }

    public List<SinrekiFilterMstDetailModel> SinrekiFilterMstDetailList { get; private set; }
}
