namespace EmrCloudApi.Requests.Receipt.RequestItem;

public class SearchByoMstRequestItem
{
    public string ByomeiCd { get; set; } = string.Empty;

    public string InputName { get; set; } = string.Empty;

    public bool IsComment { get; set; }
}
