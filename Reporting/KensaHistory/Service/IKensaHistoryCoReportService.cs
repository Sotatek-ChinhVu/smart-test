using Reporting.Mappers.Common;

namespace Reporting.KensaHistory.Service
{
    public interface IKensaHistoryCoReportService
    {
        CommonReportingRequestModel GetKensaHistoryPrintData(int hpId, long ptId, int startSinYm, int endSinYm, bool includeHoken, bool includeJihi);
    }
}
