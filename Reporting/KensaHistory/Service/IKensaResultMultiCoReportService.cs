using Reporting.Mappers.Common;

namespace Reporting.KensaHistory.Service
{
    public interface IKensaResultMultiCoReportService
    {
        CommonReportingRequestModel GetKensaResultMultiPrintData(int hpId, int userId, long ptId, int setId, int startDate, int endDate, bool showAbnormalKbn, int sinDate);
    }
}
