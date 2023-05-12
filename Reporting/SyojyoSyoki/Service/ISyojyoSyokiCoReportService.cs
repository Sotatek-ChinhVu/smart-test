using Reporting.Mappers.Common;

namespace Reporting.SyojyoSyoki.Service
{
    public interface ISyojyoSyokiCoReportService
    {
        CommonReportingRequestModel GetSyojyoSyokiReportingData(int hpId, long ptId, int seiKyuYm, int hokenId);
    }
}
