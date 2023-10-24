using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.Service
{
    public interface IP44WelfareSeikyu84CoReportService
    {
        CommonReportingRequestModel GetP44WelfareSeikyu84ReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
    }
}
