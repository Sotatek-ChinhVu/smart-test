using Domain.Models.Accounting;
using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.UpdateAccountingFormMst
{
    public class UpdateAccountingFormMstOutputData : IOutputData
    {
        public UpdateAccountingFormMstOutputData(UpdateAccountingFormMstStatus status)
        {
            Status = status;
        }

        public UpdateAccountingFormMstStatus Status { get; private set; }
    }
}