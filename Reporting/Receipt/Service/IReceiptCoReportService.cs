using Helper.Enum;
using Reporting.Receipt.Models;

namespace Reporting.Receipt.Service
{
    public interface IReceiptCoReportService
    {
        List<CoReceiptModel> GetReceiptData(int hpId, long ptId, int seikyuYm, int sinYm, int hokenId, ReceiptPreviewModeEnum mode, bool isNoCreatingReceData = false);
    }
}
