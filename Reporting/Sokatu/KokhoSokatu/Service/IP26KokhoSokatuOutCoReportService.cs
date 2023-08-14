using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service;

public interface IP26KokhoSokatuOutCoReportService
{
    CommonReportingRequestModel GetP26KokhoSokatuOutReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}
