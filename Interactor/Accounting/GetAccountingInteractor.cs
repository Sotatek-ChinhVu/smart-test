using Domain.Models.Accounting;
using UseCase.Accounting.GetAccountingInf;

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
                //Get Accounting Inf
                var syunoSeikyu = _accountingRepository.GetListSyunoSeikyu(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.RaiinNo).ToList();

                if (!syunoSeikyu.Any())
                {
                    return new GetAccountingOutputData(new List<AccountingModel>(), GetAccountingStatus.NoData);
                }
                return new GetAccountingOutputData(syunoSeikyu, GetAccountingStatus.Successed);

            }
            catch (Exception)
            {
                return new GetAccountingOutputData(new List<AccountingModel>(), GetAccountingStatus.Failed);
            }
            finally
            {
                _accountingRepository.ReleaseResource();
            }
        }

    }
}
