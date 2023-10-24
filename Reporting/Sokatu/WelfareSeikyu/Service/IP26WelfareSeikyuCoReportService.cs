using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.Service
{
    public interface IP26WelfareSeikyuCoReportService
    {
        CommonReportingRequestModel GetP26WelfareSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
    }
}
