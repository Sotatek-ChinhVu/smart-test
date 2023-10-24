namespace EmrCloudApi.Requests.Online;

public class UpdatePtInfOnlineQualifyRequest
{
    public long PtId { get; set; }

    public List<UpdatePtInfOnlineQualifyRequestItem> ResultList { get; set; }
}
