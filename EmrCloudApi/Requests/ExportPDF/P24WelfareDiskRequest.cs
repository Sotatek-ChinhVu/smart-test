namespace EmrCloudApi.Requests.ExportPDF;

public class ReceiptPrintExcelRequest : ReportRequestBase
{
    public int PrefNo { get; set; }

    public int ReportId { get; set; }

    public int ReportEdaNo { get; set; }

    public int DataKbn { get; set; }

    public int SeikyuYm { get; set; }
}
