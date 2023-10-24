using Domain.Models.Accounting;
using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.GetAccountingFormMst
{
    public class GetAccountingFormMstOutputData : IOutputData
    {
        public GetAccountingFormMstOutputData(List<AccountingFormMstModel> accountingFormMstModels, GetAccountingFormMstStatus status)
        {
            AccountingFormMstModels = accountingFormMstModels;
            Status = status;
        }
        public List<AccountingFormMstModel> AccountingFormMstModels { get; private set; }
        public GetAccountingFormMstStatus Status { get; private set; }
    }
}