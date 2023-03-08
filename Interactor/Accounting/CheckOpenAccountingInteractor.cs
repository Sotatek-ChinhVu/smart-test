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
            throw new NotImplementedException();
        }
    }
}
