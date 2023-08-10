using Reporting.Mappers.Common;

namespace Reporting.Receipt.Service
{
    public interface IReceiptCoReportService
    {
        CommonReportingRequestModel GetReceiptData(int hpId, long ptId, int sinYm, int hokenId, bool isNoCreatingReceData, bool isOpenedFromAccounting);

        CommonReportingRequestModel ShowRecePreviewAccounting(int hpId, long ptId, int sinDate, long raiinNo, int hokenId, bool isIncludeOutDrug = false);
    }
}
