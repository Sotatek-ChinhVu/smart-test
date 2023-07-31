using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service;

public interface IP17KokhoSokatuCoReportService
{
    CommonReportingRequestModel GetP17KokhoSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}
