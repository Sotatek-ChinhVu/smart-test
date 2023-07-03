namespace EmrCloudApi.Requests.Reception;

public class GetLastRaiinInfsRequest
{
    public long PtId { get; set; }

    public int SinDate { get; set; }

    public bool IsLastVisit { get; set; } = false;

}
