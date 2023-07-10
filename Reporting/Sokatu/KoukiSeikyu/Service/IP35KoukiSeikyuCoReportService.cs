using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KoukiSeikyu.Service
{
    public interface IP35KoukiSeikyuCoReportService
    {
        CommonReportingRequestModel GetP35KoukiSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
    }
}
