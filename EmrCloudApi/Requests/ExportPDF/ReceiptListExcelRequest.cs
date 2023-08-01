using Reporting.CommonMasters.Enums;
using Reporting.ReceiptList.Model;

namespace EmrCloudApi.Requests.ExportPDF
{
    public class ReceiptListExcelRequest : ReportRequestBase
    {
        public List<ReceiptInputCsvModel> receiptListModel { get; set; }
    }
}
