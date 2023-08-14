namespace EmrCloudApi.Requests.Receipt.RequestItem;

public class SaveReceCheckErrorListRequestItem
{
    public string ErrCd { get; set; } = string.Empty;

    public int SinDate { get; set; }

    public string ACd { get; set; } = string.Empty;

    public string BCd { get; set; } = string.Empty;

    public bool IsChecked { get; set; }
}
