using Domain.Models.AccountDue;
using Domain.Models.Accounting;
using Domain.Models.SystemConf;
using Helper.Common;
using UseCase.AccountDue.GetAccountDueList;
using UseCase.Accounting.CheckAccountingStatus;
using UseCase.Accounting.GetAccountingFormMst;
using UseCase.Accounting.UpdateAccountingFormMst;

namespace Interactor.Accounting
{
    public class UpdateAccountingFormMstInteractor : IUpdateAccountingFormMstInputPort
    {
        private readonly IAccountingRepository _accountingRepository;

        public UpdateAccountingFormMstInteractor(IAccountingRepository accountingRepository)
        {
            _accountingRepository = accountingRepository;
        }

        public UpdateAccountingFormMstOutputData Handle(UpdateAccountingFormMstInputData inputData)
        {
            try
            {
                _accountingRepository.UpdateAccountingFormMst(inputData.UserId, inputData.AccountingFormMstModels);
                return new UpdateAccountingFormMstOutputData(UpdateAccountingFormMstStatus.Successed);
            }
            finally
            {
                _accountingRepository.ReleaseResource();
            }
        }
    }
}
