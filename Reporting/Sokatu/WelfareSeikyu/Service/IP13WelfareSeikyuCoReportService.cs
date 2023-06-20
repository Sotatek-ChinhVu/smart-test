using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.Service
{
    public interface IP13WelfareSeikyuCoReportService
    {
        CommonReportingRequestModel GetP13WelfareSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType, int welfareType);
    }
}
