namespace EmrCloudApi.Tenant.Requests.SetMst;

public class SaveSetOrderMstRequestItem
{
    public long Id { get; set; } = 0;

    public long RpNo { get; set; } = 0;

    public long RpEdaNo { get; set; } = 0;

    public int OdrKouiKbn { get; set; } = 0;

    public string RpName { get; set; } = string.Empty;

    public int InoutKbn { get; set; } = 0;

    public int SikyuKbn { get; set; } = 0;

    public int SyohoSbt { get; set; } = 0;

    public int SanteiKbn { get; set; } = 0;

    public int TosekiKbn { get; set; } = 0;

    public int DaysCnt { get; set; } = 0;

    public int SortNo { get; set; } = 0;

    public int IsDeleted { get; set; } = 0;

    public List<SetOrderInfDetailRequestItem> OrdInfDetails { get; set; } = new();
}
