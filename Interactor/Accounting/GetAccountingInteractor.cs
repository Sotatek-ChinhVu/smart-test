using Domain.Models.Accounting;
using UseCase.Accounting;

namespace Interactor.Accounting
{
    public class GetAccountingInteractor : IGetAccountingInputPort
    {
        private readonly IAccountingRepository _accountingRepository;

        public GetAccountingInteractor(IAccountingRepository accountingRepository)
        {
            _accountingRepository = accountingRepository;
        }

        public GetAccountingOutputData Handle(GetAccountingInputData inputData)
        {
            try
            {
                var syunoSeikyu = _accountingRepository.GetListSyunoSeikyu(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.RaiinNo).ToList();

                if (!syunoSeikyu.Any())
                {
                    return new GetAccountingOutputData(new AccountingInfModel(), GetAccountingStatus.NoData);
                }
                var accountingInf = _accountingRepository.GetAccountingInfAllRaiinNo(syunoSeikyu);

                return new GetAccountingOutputData(accountingInf, GetAccountingStatus.Successed);

            }
            catch (Exception)
            {
                return new GetAccountingOutputData(new AccountingInfModel(), GetAccountingStatus.Failed);
            }
        }
    }
}
