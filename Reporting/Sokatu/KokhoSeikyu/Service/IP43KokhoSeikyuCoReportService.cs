using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSeikyu.Service;

public interface IP43KokhoSeikyuCoReportService
{
    CommonReportingRequestModel GetP43KokhoSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}
