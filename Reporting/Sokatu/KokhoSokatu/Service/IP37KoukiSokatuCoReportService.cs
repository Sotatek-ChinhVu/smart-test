using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service
{
    public interface IP37KoukiSokatuCoReportService
    {
        CommonReportingRequestModel GetP37KoukiSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
    }
}
