using Domain.Models.Accounting;
using UseCase.Core.Sync.Core;

namespace UseCase.Accounting
{
    public class GetAccountingOutputData : IOutputData
    {
        public GetAccountingOutputData(AccountingModel accountingModel, GetAccountingStatus getAccountingStatus)
        {
            AccountingModel = accountingModel;
            GetAccountingStatus = getAccountingStatus;
        }

        public AccountingModel AccountingModel { get; private set; }
        public GetAccountingStatus GetAccountingStatus { get; private set; }
    }
}
