using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KoukiSeikyu.Service
{
    public interface IP12KoukiSeikyuCoReportService
    {
        CommonReportingRequestModel GetP12KoukiSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
    }
}
