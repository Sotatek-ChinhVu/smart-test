using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service;

public interface IP44KokhoSokatuCoReportService
{
    CommonReportingRequestModel GetP44KokhoSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}
