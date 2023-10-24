using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.Service
{
    public interface IP35WelfareSeikyuCoReportService
    {
        CommonReportingRequestModel GetP35WelfareSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
    }
}
