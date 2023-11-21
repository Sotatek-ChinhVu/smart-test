using Reporting.Mappers.Common;

namespace Reporting.KensaHistory.Service
{
    public interface IKensaHistoryCoReportService
    {
        CommonReportingRequestModel GetKensaHistoryPrintData(int hpId, int userId, long ptId, int setId, int seikyuYm, int startDate, int endDate, bool showAbnormalKbn, int sinDate, bool checkAbnormalKbn);
    }
}
