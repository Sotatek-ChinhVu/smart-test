using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSeikyu.Service
{
    public interface IP37KokhoSeikyuCoReportService
    {
        CommonReportingRequestModel GetP37KokhoSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType, List<string> printHokensyaNos);
    }
}
