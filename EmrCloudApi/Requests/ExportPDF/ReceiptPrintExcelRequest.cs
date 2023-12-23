using Reporting.CommonMasters.Enums;
using Reporting.ReceiptList.Model;

namespace EmrCloudApi.Requests.ExportPDF;

public class ReceiptPrintExcelRequest
{
    public int PrefNo { get; set; }

    public int ReportId { get; set; }

    public int ReportEdaNo { get; set; }

    public int DataKbn { get; set; }

    public int SeikyuYm { get; set; }
}
