using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KoukiSeikyu.Service;

public interface IP13KoukiSeikyuCoReportService
{
    CommonReportingRequestModel GetP13KoukiSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}
