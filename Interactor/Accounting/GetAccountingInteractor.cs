using Domain.Models.Accounting;
using Infrastructure.Repositories;
using UseCase.Accounting;

namespace Interactor.Accounting
{
    public class GetAccountingInteractor : GetAccountingInputPort
    {
        private readonly AccountingRepository _accountingRepository;

        public GetAccountingInteractor(AccountingRepository accountingRepository)
        {
            _accountingRepository = accountingRepository;
        }

        public GetAccountingOutputData Handle(GetAccountingInputData inputData)
        {
            try
            {
                var syunoSeikyu = _accountingRepository.GetListSyunoSeikyu(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.RaiinNo).FirstOrDefault();

                if (syunoSeikyu != null)
                {
                    return new GetAccountingOutputData(syunoSeikyu, GetAccountingStatus.Successed);
                }
            }
            catch (Exception)
            {
                return new GetAccountingOutputData(new AccountingModel(), GetAccountingStatus.Failed);
            }
        }
    }
}
