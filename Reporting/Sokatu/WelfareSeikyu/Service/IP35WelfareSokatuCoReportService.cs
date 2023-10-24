using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.Service
{
    public interface IP35WelfareSokatuCoReportService
    {
        CommonReportingRequestModel GetP35WelfareSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
    }
}
