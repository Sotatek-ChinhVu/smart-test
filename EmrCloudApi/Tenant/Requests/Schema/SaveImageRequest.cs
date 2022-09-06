namespace EmrCloudApi.Tenant.Requests.Schema;

public class SaveImageRequest
{
    public long PtId { get; set; }

    public string OldImage { get; set; } = string.Empty;
}
