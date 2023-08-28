using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.Service
{
    public interface IP43KikuchiSeikyu41CoReportService
    {
        CommonReportingRequestModel GetP43KikuchiSeikyu41ReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
    }
}
