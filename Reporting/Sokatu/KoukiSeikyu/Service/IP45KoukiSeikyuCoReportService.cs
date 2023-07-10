using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KoukiSeikyu.Service;

public interface IP45KoukiSeikyuCoReportService
{
    CommonReportingRequestModel GetP45KoukiSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}
