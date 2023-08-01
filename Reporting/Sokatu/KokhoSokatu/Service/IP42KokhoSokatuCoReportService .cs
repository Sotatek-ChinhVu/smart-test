using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service;

public interface IP42KokhoSokatuCoReportService
{
    CommonReportingRequestModel GetP42KokhoSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}
