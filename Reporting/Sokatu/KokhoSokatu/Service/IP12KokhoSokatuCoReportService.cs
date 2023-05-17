using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service
{
    public interface IP12KokhoSokatuCoReportService
    {
        CommonReportingRequestModel GetP12KokhoSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
    }
}
