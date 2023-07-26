using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service;

public interface IP41KokhoSokatuCoReportService
{
    CommonReportingRequestModel GetP41KokhoSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}
