namespace EmrCloudApi.Requests.Document;

public class DeleteDocTemplateRequest
{
    public int CategoryCd { get; set; }

    public string FileTemplateName { get; set; } = string.Empty;
}

