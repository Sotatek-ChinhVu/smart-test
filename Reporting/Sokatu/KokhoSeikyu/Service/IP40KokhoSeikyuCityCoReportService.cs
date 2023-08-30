using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSeikyu.Service
{
    public interface IP40KokhoSeikyuCityCoReportService
    {
        CommonReportingRequestModel GetP40KokhoSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType, List<string> printHokensyaNos);
    }
}
