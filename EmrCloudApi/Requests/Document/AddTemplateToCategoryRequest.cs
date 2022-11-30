namespace EmrCloudApi.Requests.Document;

public class AddTemplateToCategoryRequest
{
    public string FileName { get; set; } = string.Empty;

    public int CategoryCd { get; set; }
}
