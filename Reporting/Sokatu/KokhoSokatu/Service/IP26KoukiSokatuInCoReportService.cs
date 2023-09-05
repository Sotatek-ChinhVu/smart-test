using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service
{
    public interface IP26KoukiSokatuInCoReportService
    {
        CommonReportingRequestModel GetP26KoukiSokatuInReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
    }
}
