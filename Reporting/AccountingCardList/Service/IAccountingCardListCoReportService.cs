using Reporting.AccountingCardList.Model;
using Reporting.Mappers.Common;

namespace Reporting.AccountingCardList.Service;

public interface IAccountingCardListCoReportService
{
    CommonReportingRequestModel GetAccountingCardListData(int hpId, List<TargetItem> targets, bool includeOutDrug, string kaName, string tantoName, string uketukeSbt, string hoken);
}
