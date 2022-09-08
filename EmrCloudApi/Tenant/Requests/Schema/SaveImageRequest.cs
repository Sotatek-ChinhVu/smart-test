namespace EmrCloudApi.Tenant.Requests.Schema;

public class SaveImageRequest
{
    public int HpId { get; set; }
    public long PtId { get; set; }
    public long RaiinNo { get; set; }
    public string OldImage { get; set; } = string.Empty;
}
