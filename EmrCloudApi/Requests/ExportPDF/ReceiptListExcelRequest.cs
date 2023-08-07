using Reporting.CommonMasters.Enums;
using Reporting.ReceiptList.Model;

namespace EmrCloudApi.Requests.ExportPDF
{
    public class ReceiptListExcelRequest : ReportRequestBase
    {
        public bool IsIsExportTitle { get; set; }
        public List<ReceiptInputCsvModel> receiptListModel { get; set; } = new();
    }
}
