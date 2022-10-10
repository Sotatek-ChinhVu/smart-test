namespace EmrCloudApi.Tenant.Requests.Ka;

public class SaveListKaMstRequest
{
    public int HpId { get; set; } = 1;
    public int UserId { get; set; } = 0;
    public List<KaMstRequestItem> kaMstRequestItems { get; set; } = new();
}
