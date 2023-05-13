using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service
{
    public interface IP08KokhoSokatuCoReportService
    {
        CommonReportingRequestModel GetP08KokhoSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
    }
}
