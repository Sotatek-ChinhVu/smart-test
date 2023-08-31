using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSeikyu.Service
{
    public interface IP33KokhoSeikyuCoReportService
    {
        CommonReportingRequestModel GetP33KokhoSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
    }
}
