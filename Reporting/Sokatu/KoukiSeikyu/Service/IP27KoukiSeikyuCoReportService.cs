using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KoukiSeikyu.Service;

public interface IP27KoukiSeikyuCoReportService
{
    CommonReportingRequestModel GetP27KoukiSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType, PrefKbn prefKbn);
}
