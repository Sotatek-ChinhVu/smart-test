using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.Service
{
    public interface IP43KikuchiSeikyu43CoReportService
    {
        CommonReportingRequestModel GetP43KikuchiSeikyu43ReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
    }
}
