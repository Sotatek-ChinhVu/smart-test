using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service;

public interface IP33KokhoSokatuCoReportService
{
    CommonReportingRequestModel GetP33KokhoSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}
