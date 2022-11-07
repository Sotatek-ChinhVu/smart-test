namespace EmrCloudApi.Tenant.Requests.Schema;

public class SaveImageTodayOrderRequest
{
    public long PtId { get; set; }
    public long RaiinNo { get; set; }
    public string OldImage { get; set; } = string.Empty;
}
