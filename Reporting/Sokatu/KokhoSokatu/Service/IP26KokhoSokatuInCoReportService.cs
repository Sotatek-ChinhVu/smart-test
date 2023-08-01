using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service;

public interface IP26KokhoSokatuInCoReportService
{
    CommonReportingRequestModel GetP26KokhoSokatuInReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}
