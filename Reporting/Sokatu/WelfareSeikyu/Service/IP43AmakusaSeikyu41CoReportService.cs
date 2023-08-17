using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.Service
{
    public interface IP43AmakusaSeikyu41CoReportService
    {
        CommonReportingRequestModel GetP43AmakusaSeikyu41sReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
    }
}
