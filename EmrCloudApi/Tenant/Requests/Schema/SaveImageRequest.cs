namespace EmrCloudApi.Tenant.Requests.Schema;

public class SaveImageRequest
{
    public string OldImage { get; set; } = string.Empty;

    public string FileName { get; set; } = string.Empty;

    public Stream StreamImage { get; set; } = Stream.Null;
}
