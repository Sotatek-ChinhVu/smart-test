using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSeikyu.Service;

public interface IP13KokhoSeikyuCoReportService
{
    CommonReportingRequestModel GetP13KokhoSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}
