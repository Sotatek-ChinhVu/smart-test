namespace EmrCloudApi.Requests.Document;

public class UploadTemplateToCategoryRequest
{
    public string FileName { get; set; } = string.Empty;

    public int CategoryCd { get; set; }

    public bool OverWrite { get; set; }
}
