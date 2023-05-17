using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service
{
    public interface IP28KokhoSokatuCoReportService
    {
        CommonReportingRequestModel GetP28KokhoSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
    }
}
