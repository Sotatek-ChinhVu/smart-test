namespace EmrCloudApi.Requests.ExportPDF;

public class ReceiptPreviewRequest : ReportRequestBase
{
    public long PtId { get; set; }

    public int SinYm { get; set; }

    public int HokenId { get; set; }

    public int SinDate { get; set; }

    public long RaiinNo { get; set; }

    public bool isOpenedFromAccounting { get; set; } = true;
}
