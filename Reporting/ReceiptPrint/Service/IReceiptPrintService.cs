using Reporting.Mappers.Common;

namespace Reporting.ReceiptPrint.Service
{
    public interface IReceiptPrintService
    {
        CommonReportingRequestModel GetReceiptPrint(int hpId, int prefNo, int reportId, int reportEdaNo, int dataKbn, int ptId, int seikyuYm, int sinYm, int hokenId, int diskKind, int diskCnt);
    }
}
