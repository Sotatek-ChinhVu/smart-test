namespace EmrCloudApi.Requests.ExportPDF;

public class ReceiptPrintRequest : ReportRequestBase
{
    public int PrefNo { get; set; }

    public int ReportId { get; set; }

    public int ReportEdaNo { get; set; }

    public int DataKbn { get; set; }

    public int PtId { get; set; }

    public int SeikyuYm { get; set; }

    public int SinYm { get; set; }

    public int HokenId { get; set; }

    public int DiskKind { get; set; }

    public int DiskCnt { get; set; }

    public int WelfareType { get; set; }

    public string FormName { get; set; } = string.Empty;
}
