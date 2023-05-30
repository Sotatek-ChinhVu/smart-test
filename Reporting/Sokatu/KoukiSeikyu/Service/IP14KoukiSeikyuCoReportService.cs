using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KoukiSeikyu.Service
{
    public interface IP14KoukiSeikyuCoReportService
    {
        CommonReportingRequestModel GetP14KoukiSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
    }
}
