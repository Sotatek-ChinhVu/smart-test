using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KoukiSeikyu.Service
{
    public interface IP44KoukiSeikyuCoReportService
    {
        CommonReportingRequestModel GetP44KoukiSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
    }
}
