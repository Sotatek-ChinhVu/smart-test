namespace EmrCloudApi.Requests.Family;

public class GetListFamilyReverserRequest
{
    public long FamilyPtId { get; set; }

    public Dictionary<long, string> DicPtInf { get; set; } = new();
}
