using Domain.Models.Accounting;
using UseCase.Core.Sync.Core;

namespace UseCase.Accounting
{
    public class GetAccountingOutputData : IOutputData
    {
        public GetAccountingOutputData(List<RaiinInfModel> raiinInfModels, List<AccountingModel> accountingModel, AccountingInfModel accountingInfModel, GetAccountingStatus getAccountingStatus)
        {
            RaiinInfModels = raiinInfModels;
            AccountingModel = accountingModel;
            AccountingInfModel = accountingInfModel;
            GetAccountingStatus = getAccountingStatus;
        }

        public List<RaiinInfModel> RaiinInfModels { get; private set; }
        public List<AccountingModel> AccountingModel { get; private set; }
        public AccountingInfModel AccountingInfModel { get; private set; }
        public GetAccountingStatus GetAccountingStatus { get; private set; }
    }
}
