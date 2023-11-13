namespace EmrCloudApi.Requests.ExportPDF;

public class SetDownloadNameReportRequest
{
    public string DownloadName { get; set; } = string.Empty;

    public string InputBase64File { get; set; } = string.Empty;
}
