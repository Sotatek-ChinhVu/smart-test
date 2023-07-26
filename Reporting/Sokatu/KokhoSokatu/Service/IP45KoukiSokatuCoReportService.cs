using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service;

public interface IP45KoukiSokatuCoReportService
{
    CommonReportingRequestModel GetP45KoukiSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}
