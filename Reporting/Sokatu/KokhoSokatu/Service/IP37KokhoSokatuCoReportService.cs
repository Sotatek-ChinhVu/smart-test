using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service;

public interface IP37KokhoSokatuCoReportService
{
    CommonReportingRequestModel GetP37KokhoSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}
