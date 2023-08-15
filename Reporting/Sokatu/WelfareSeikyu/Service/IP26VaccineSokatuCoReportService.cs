using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.Service
{
    public interface IP26VaccineSokatuCoReportService
    {
        CommonReportingRequestModel GetP26VaccineSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
    }
}
