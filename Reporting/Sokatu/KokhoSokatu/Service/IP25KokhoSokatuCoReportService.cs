using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service;

public interface IP25KokhoSokatuCoReportService
{
    CommonReportingRequestModel GetP25KokhoSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType, int diskKind, int diskCnt);
}
