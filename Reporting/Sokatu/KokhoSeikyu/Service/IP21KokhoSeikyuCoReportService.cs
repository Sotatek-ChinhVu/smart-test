using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSeikyu.Service;

public interface IP21KokhoSeikyuCoReportService
{
    CommonReportingRequestModel GetP21KokhoSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}
