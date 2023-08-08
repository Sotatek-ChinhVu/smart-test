using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.Service
{
    public interface IP43KumamotoSeikyuCoReportService
    {
        CommonReportingRequestModel GetP43KumamotoSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType, int welfareType);
    }
}
