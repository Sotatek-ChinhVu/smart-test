using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KoukiSeikyu.Service;

public interface IP42KoukiSeikyuCoReportService
{
    CommonReportingRequestModel GetP42KoukiSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}
