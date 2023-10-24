using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.Service
{
    public interface IP27IzumisanoSeikyuCoReportService
    {
        CommonReportingRequestModel GetP27IzumisanoSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
    }
}
