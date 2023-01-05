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
                var syunoSeikyu = _accountingRepository.GetListSyunoSeikyu(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.RaiinNo).FirstOrDefault();


                return new GetAccountingOutputData(syunoSeikyu, GetAccountingStatus.Successed);

            }
            catch (Exception)
            {
                return new GetAccountingOutputData(new AccountingModel(), GetAccountingStatus.Failed);
            }
        }
    }
}
