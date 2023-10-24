using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.Service
{
    public interface IP24WelfareSofuPaperCoReportService
    {
        CommonReportingRequestModel GetP24WelfareSofuPaperReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
    }
}
