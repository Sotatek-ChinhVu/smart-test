namespace EmrCloudApi.Requests.DrugInfor.SaveSinrekiFilterMstListRequestItem;

public class SinrekiFilterMstDetailRequestItem
{
    public long Id { get; set; }

    public string ItemCd { get; set; } = string.Empty;

    public int SortNo { get; set; }

    public bool IsExclude { get; set; }

    public bool IsDeleted { get; set; }
}
