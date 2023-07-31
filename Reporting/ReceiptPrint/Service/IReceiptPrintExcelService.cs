using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.ReceiptList.Model;

namespace Reporting.ReceiptPrint.Service
{
    public interface IReceiptPrintExcelService
    {
        CommonExcelReportingModel GetReceiptPrintExcel(int hpId, int prefNo, int reportId, int reportEdaNo, int dataKbn, int seikyuYm, List<ReceiptListModel> receiptListModel, CoFileType printType);
    }
}
