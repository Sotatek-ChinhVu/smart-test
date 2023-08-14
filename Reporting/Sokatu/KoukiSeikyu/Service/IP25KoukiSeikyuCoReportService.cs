using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KoukiSeikyu.Service;

public interface IP25KoukiSeikyuCoReportService
{
    CommonReportingRequestModel GetP25KoukiSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}
