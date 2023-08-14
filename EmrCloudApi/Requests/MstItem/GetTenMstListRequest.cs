namespace EmrCloudApi.Requests.MstItem;

public class GetTenMstListRequest
{
    public int SinDate { get; set; }

    public List<string> ItemCdList { get; set; } = new();
}
