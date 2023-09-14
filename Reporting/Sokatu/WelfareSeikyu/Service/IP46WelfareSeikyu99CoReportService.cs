using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.Service
{
    public interface IP46WelfareSeikyu99CoReportService
    {
        CommonReportingRequestModel GetP46WelfareSeikyu99ReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
    }
}
