namespace EmrCloudApi.Requests.ExportPDF;

public class CheckExistTemplateAccountingRequest
{
    public string TemplateName { get; set; } = string.Empty;

    public int PrintType { get; set; }
}
