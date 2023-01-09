using Domain.Models.Accounting;
using UseCase.Core.Sync.Core;

namespace UseCase.Accounting
{
    public class GetAccountingOutputData : IOutputData
    {
        public GetAccountingOutputData(AccountingInfModel accountingInfModel, GetAccountingStatus getAccountingStatus)
        {
            AccountingInfModel = accountingInfModel;
            GetAccountingStatus = getAccountingStatus;
        }

        public AccountingInfModel AccountingInfModel { get; private set; }
        public GetAccountingStatus GetAccountingStatus { get; private set; }
    }
}
