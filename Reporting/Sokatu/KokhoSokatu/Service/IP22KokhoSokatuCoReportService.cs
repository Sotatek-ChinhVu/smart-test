using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service;

public interface IP22KokhoSokatuCoReportService
{
    CommonReportingRequestModel GetP22KokhoSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}