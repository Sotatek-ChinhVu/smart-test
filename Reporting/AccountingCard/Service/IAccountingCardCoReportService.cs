using Reporting.Mappers.Common;

namespace Reporting.AccountingCard.Service
{
    public interface IAccountingCardCoReportService
    {
        CommonReportingRequestModel GetAccountingCardReportingData(int hpId, long ptId, int sinYm, int hokenId, bool includeOutDrug);
    }
}
