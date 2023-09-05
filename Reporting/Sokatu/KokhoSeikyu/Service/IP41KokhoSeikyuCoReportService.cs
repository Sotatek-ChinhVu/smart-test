using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSeikyu.Service
{
    public interface IP41KokhoSeikyuCoReportService
    {
        CommonReportingRequestModel GetP40KokhoSeikyuKumiaiReportingData(int hpId, int seikyuYm, SeikyuType seikyuType, List<string> printHokensyaNos);
    }
}
