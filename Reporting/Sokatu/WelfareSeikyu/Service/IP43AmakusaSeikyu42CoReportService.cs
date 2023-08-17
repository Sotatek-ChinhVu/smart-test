using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.Service
{
    public interface IP43AmakusaSeikyu42CoReportService
    {
        CommonReportingRequestModel GetP43AmakusaSeikyu42ReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
    }
}
