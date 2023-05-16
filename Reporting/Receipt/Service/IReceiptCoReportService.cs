using Reporting.Mappers.Common;

namespace Reporting.Receipt.Service
{
    public interface IReceiptCoReportService
    {
        CommonReportingRequestModel GetReceiptData(int hpId, long ptId, int sinYm, int hokenId, bool isNoCreatingReceData = false);
    }
}
