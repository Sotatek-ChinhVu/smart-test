using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.Service
{
    public interface IP23NagoyaSeikyuCoReportService
    {
        CommonReportingRequestModel GetP23NagoyaSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
    }
}
