using Reporting.Mappers.Common;

namespace Reporting.KensaHistory.Service
{
    public interface IKensaHistoryCoReportService
    {
        CommonReportingRequestModel GetKensaHistoryPrintData(int hpId, int userId, long ptId, int setId, int iraiCd, int startDate, bool showAbnormalKbn, int itemQuantity);
    }
}
