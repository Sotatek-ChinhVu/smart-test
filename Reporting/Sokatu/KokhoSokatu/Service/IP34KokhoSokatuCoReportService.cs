using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service;

public interface IP34KokhoSokatuCoReportService
{
    CommonReportingRequestModel GetP34KokhoSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}
