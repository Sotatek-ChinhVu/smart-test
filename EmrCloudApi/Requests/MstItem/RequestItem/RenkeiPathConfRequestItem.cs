namespace EmrCloudApi.Requests.MstItem.RequestItem;

public class RenkeiPathConfRequestItem
{
    public long Id { get; set; }

    public int EdaNo { get; set; }

    public string Path { get; set; } = string.Empty;

    public string Machine { get; set; } = string.Empty;

    public int CharCd { get; set; }

    public string WorkPath { get; set; } = string.Empty;

    public int Interval { get; set; }

    public string Param { get; set; } = string.Empty;

    public string User { get; set; } = string.Empty;

    public string PassWord { get; set; } = string.Empty;

    public int IsInvalid { get; set; }

    public string Biko { get; set; } = string.Empty;

    public bool IsDeleted { get; set; }
}
