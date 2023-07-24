using Reporting.Mappers.Common;

namespace Reporting.ReceiptPrint.Service
{
    public interface IReceiptPrintExcelService
    {
        CommonExcelReportingModel GetReceiptPrintExcel(int hpId, int prefNo, int reportId, int reportEdaNo, int dataKbn, int seikyuYm);
    }
}
