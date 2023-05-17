using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service
{
    public interface IP11KokhoSokatuCoReportService
    {
        CommonReportingRequestModel GetP11KokhoSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
    }
}
