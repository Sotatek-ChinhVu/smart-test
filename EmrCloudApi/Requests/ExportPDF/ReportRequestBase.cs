namespace EmrCloudApi.Requests.ExportPDF;

public class ReportRequestBase
{
    public int HpId { get; set; }
    public string FormName { get; set; } = string.Empty;
}
