using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSeikyu.Service
{
    public interface IP40KokhoSeikyuKumiaiCoReportService
    {
        CommonReportingRequestModel GetP40KokhoSeikyuKumiaiReportingData(int hpId, int seikyuYm, SeikyuType seikyuType, List<string> printHokensyaNos);
    }
}
