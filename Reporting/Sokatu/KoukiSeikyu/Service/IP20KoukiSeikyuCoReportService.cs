using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KoukiSeikyu.Service
{
    public interface IP20KoukiSeikyuCoReportService
    {
        CommonReportingRequestModel GetP20KoukiSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
    }
}
