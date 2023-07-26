using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service;

public interface IP43KoukiSokatuCoReportService
{
    CommonReportingRequestModel GetP43KoukiSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}
