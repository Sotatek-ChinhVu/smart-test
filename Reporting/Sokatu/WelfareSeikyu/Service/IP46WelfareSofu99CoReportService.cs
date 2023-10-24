using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.Service
{
    public interface IP46WelfareSofu99CoReportService
    {
        CommonReportingRequestModel GetP46WelfareSofu99ReportingData(int hpId, int seikyuYm, SeikyuType seikyuType, int welfareType);
    }
}
