using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service;

public interface IP23KokhoSokatuCoReportService
{
    CommonReportingRequestModel GetP23KokhoSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}
