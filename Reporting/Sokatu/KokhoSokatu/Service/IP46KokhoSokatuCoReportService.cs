using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service
{
    public interface IP46KokhoSokatuCoReportService
    {
        CommonReportingRequestModel GetP46KokhoSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
    }
}
