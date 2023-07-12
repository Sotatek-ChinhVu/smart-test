using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KoukiSeikyu.Service;

public interface IP23KoukiSeikyuCoReportService
{
    CommonReportingRequestModel GetP23KoukiSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}
