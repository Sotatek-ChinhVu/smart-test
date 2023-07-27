using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service;

public interface IP43KokhoSokatuCoReportService
{
    CommonReportingRequestModel GetP43KokhoSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}
