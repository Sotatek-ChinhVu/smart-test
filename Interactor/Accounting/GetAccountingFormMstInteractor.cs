using Domain.Models.AccountDue;
using Domain.Models.Accounting;
using Domain.Models.SystemConf;
using Helper.Common;
using UseCase.AccountDue.GetAccountDueList;
using UseCase.Accounting.CheckAccountingStatus;
using UseCase.Accounting.GetAccountingFormMst;

namespace Interactor.Accounting
{
    public class GetAccountingFormMstInteractor : IGetAccountingFormMstInputPort
    {
        private readonly IAccountingRepository _accountingRepository;

        public GetAccountingFormMstInteractor(IAccountingRepository accountingRepository)
        {
            _accountingRepository = accountingRepository;
        }

        public GetAccountingFormMstOutputData Handle(GetAccountingFormMstInputData inputData)
        {
            try
            {
                var results = _accountingRepository.GetAccountingFormMstModels(inputData.HpId);
                return new GetAccountingFormMstOutputData(results, GetAccountingFormMstStatus.Successed);
            }
            finally
            {
                _accountingRepository.ReleaseResource();
            }
        }
    }
}
