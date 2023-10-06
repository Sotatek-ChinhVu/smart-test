using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service;

public interface IP45KokhoSokatuCoReportService
{
    CommonReportingRequestModel GetP45KokhoSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}
