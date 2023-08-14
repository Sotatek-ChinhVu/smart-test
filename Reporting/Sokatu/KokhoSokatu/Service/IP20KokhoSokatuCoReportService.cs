using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service;

public interface IP20KokhoSokatuCoReportService
{
    CommonReportingRequestModel GetP20KokhoSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}
