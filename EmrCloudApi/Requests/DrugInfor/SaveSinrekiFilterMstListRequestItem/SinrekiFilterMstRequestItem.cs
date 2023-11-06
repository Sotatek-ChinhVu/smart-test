namespace EmrCloudApi.Requests.DrugInfor.SaveSinrekiFilterMstListRequestItem;

public class SinrekiFilterMstRequestItem
{
    public int GrpCd { get; set; }

    public string Name { get; set; } = string.Empty;

    public int SortNo { get; set; }

    public List<SinrekiFilterMstKouiRequestItem> SinrekiFilterMstKouiList { get; set; } = new();

    public List<SinrekiFilterMstDetailRequestItem> SinrekiFilterMstDetailList { get; set; } = new();

    public bool IsDeleted { get; set; }
}
