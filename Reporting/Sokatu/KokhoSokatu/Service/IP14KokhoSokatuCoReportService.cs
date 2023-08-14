using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service;

public interface IP14KokhoSokatuCoReportService
{
    CommonReportingRequestModel GetP14KokhoSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}
