namespace EmrCloudApi.Requests.ExportPDF;

public class YakutaiRequest : ReportRequestBase
{
    public long PtId { get; set; }
    public int SinDate { get; set; }
    public int RaiinNo { get; set; }
}
