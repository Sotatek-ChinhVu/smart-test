using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.Service
{
    public interface IP43ReihokuSeikyu41CoReportService
    {
        CommonReportingRequestModel GetP43ReihokuSeikyu41ReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
    }
}
