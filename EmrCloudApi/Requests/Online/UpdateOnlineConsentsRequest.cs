namespace EmrCloudApi.Requests.Online;

public class UpdateOnlineConsentsRequest
{
    public long PtId { get; set; }

    public List<string> ResponseList { get; set; } = new();
}
