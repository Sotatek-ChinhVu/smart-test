namespace EmrCloudApi.Tenant.Requests.Reception;

public class GetReceptionListRequest
{
    public int HpId { get; set; }
    public int SinDate { get; set; }
    public List<int> GrpIds { get; set; } = new List<int>();
}
