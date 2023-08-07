using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Receipt.Service
{
    public interface IReceiptCoReportService
    {
        CommonReportingRequestModel GetReceiptData(int hpId, long ptId, int sinYm, int hokenId, bool isNoCreatingReceData = false);

        CommonReportingRequestModel GetReceiptDataByReceiptCheckList(int hpId,
            int seikyuYm, List<long> ptId, int sinYm, int hokenId, int kaId, int tantoId,
            int target, string receSbt, int printNoFrom, int printNoTo,
            SeikyuType seikyuType, bool includeTester, bool includeOutDrug,
            int sort
        );
    }
}
