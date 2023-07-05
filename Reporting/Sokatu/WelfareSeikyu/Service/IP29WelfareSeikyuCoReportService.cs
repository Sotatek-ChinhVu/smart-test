using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.Service
{
    public interface IP29WelfareSeikyuCoReportService
    {
        CommonReportingRequestModel GetP29WelfareSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
    }
}
