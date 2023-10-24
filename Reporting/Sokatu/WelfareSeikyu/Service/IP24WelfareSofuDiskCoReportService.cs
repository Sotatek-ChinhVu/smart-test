using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.Service
{
    public interface IP24WelfareSofuDiskCoReportService
    {
        CommonReportingRequestModel GetP24WelfareSofuDiskReportingData(int hpId, int seikyuYm, SeikyuType seikyuType, int diskCnt);
    }
}
