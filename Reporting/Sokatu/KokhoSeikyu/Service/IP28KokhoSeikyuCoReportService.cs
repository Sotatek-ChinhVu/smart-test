using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSeikyu.Service;

public interface IP28KokhoSeikyuCoReportService
{
    CommonReportingRequestModel GetP28KokhoSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}
