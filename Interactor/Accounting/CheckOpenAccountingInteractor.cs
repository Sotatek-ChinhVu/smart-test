using Domain.Models.Accounting;
using UseCase.Accounting.CheckOpenAccounting;

namespace Interactor.Accounting
{
    public class CheckOpenAccountingInteractor : ICheckOpenAccountingInputPort
    {
        private readonly IAccountingRepository _accountingRepository;
        public CheckOpenAccountingInteractor(IAccountingRepository accountingRepository)
        {
            _accountingRepository = accountingRepository;
        }
        public CheckOpenAccountingOutputData Handle(CheckOpenAccountingInputData inputData)
        {
            var checkIsOpen = _accountingRepository.CheckIsOpenAccounting(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.RaiinNo);

            if (checkIsOpen == byte.MinValue)
            {
                return new CheckOpenAccountingOutputData(CheckOpenAccountingStatus.NoPaymentInfo);
            }
            else if (checkIsOpen == 2)
            {
                return new CheckOpenAccountingOutputData(CheckOpenAccountingStatus.TryAgainLater);
            }

            return new CheckOpenAccountingOutputData(CheckOpenAccountingStatus.Successed);
        }

    }
}
