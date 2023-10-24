namespace EmrCloudApi.Requests.MainMenu.RequestItem;

public class StatisticMenuRequestItem
{
    public int MenuId { get; set; }

    public int ReportId { get; set; }

    public int SortNo { get; set; }

    public string MenuName { get; set; } = string.Empty;

    public int IsPrint { get; set; }

    public List<StaConfRequestItem> StaConfigList { get; set; } = new();

    public bool IsDeleted { get; set; }

    public bool IsSaveTemp { get; set; }
}
