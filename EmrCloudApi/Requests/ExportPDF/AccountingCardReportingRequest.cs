namespace EmrCloudApi.Requests.ExportPDF;

public class AccountingCardReportingRequest
{
    public long PtId { get; set; }
    public int SinYm { get; set; }
    public int HokenId { get; set; }
    public bool IncludeOutDrug { get; set; }
}
