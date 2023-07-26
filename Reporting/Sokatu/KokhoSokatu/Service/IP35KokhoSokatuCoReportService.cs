using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service;

public interface IP35KokhoSokatuCoReportService
{
    CommonReportingRequestModel GetP35KokhoSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}
