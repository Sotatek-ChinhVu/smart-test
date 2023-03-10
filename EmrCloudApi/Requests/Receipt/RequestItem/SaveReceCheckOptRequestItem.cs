namespace EmrCloudApi.Requests.Receipt.RequestItem;

public class SaveReceCheckOptRequestItem
{
    public string ErrCd { get; set; } = string.Empty;

    public int CheckOpt { get; set; }

    public bool IsInvalid { get; set; }
}
