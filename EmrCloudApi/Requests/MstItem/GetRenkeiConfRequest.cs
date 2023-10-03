namespace EmrCloudApi.Requests.MstItem;

public class GetRenkeiConfRequest
{
    public int RenkeiSbt { get; set; }

    public bool NotLoadMst { get; set; } = false;
}
