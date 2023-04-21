using Helper.Enum;
using Reporting.Mappers.Common;

namespace Reporting.Receipt.Service
{
    public interface IReceiptCoReportService
    {
        CommonReportingRequestModel GetReceiptData(int hpId, long ptId, int seikyuYm, int sinYm, int hokenId, ReceiptPreviewModeEnum mode, bool isNoCreatingReceData = false);
    }
}
