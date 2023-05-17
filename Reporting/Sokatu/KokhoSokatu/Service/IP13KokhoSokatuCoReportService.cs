using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service
{
    public interface IP13KokhoSokatuCoReportService
    {
        CommonReportingRequestModel GetP13KokhoSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType, int diskKind, int diskCnt);
    }
}
