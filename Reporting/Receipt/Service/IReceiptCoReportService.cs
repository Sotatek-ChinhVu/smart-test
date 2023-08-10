using Reporting.Mappers.Common;

namespace Reporting.Receipt.Service
{
    public interface IReceiptCoReportService
    {
        CommonReportingRequestModel ShowRecePreviewAccounting(int hpId, long ptId, int sinDate, long raiinNo, int hokenId, bool isIncludeOutDrug = false);

        CommonReportingRequestModel GetReceiptDataFromReceCheck(int hpId, long ptId, int sinYm, int seikyuYm, int hokenId, int hokenKbn);
    }
}
