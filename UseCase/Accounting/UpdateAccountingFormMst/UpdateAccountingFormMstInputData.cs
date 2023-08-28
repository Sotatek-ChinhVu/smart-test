using Domain.Models.Accounting;
using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.UpdateAccountingFormMst
{
    public class UpdateAccountingFormMstInputData : IInputData<UpdateAccountingFormMstOutputData>
    {
        public UpdateAccountingFormMstInputData(int userId, List<AccountingFormMstModel> accountingFormMstModels)
        {
            UserId = userId;
            AccountingFormMstModels = accountingFormMstModels;
        }

        public int UserId { get; private set; }

        public List<AccountingFormMstModel> AccountingFormMstModels{ get; private set; }
    }
}
