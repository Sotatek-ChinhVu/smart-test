namespace EmrCloudApi.Requests.ExportPDF;

public class MemoMsgPrintRequest
{
    public string ReportName { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public List<string> ListMessage { get; set; } = new();

    public string FileName { get; set; } = string.Empty;
}
