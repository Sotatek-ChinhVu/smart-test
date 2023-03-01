namespace EmrCloudApi.Requests.Family;

public class GetFamilyReverserListRequest
{
    public long FamilyPtId { get; set; }

    public Dictionary<long, string> DicPtInf { get; set; } = new();
}
