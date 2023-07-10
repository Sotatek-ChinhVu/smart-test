using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KoukiSeikyu.Service;

public interface IP22KoukiSeikyuCoReportService
{
    CommonReportingRequestModel GetP22KoukiSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}
