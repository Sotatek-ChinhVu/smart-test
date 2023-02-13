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


                //Get Accounting Inf
                var syunoSeikyu = _accountingRepository.GetListSyunoSeikyu(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.RaiinNo).ToList();

                if (!syunoSeikyu.Any())
                {
                    return new GetAccountingOutputData(new List<AccountingModel>(), new AccountingInfModel(), new List<PtByomeiModel>(), GetAccountingStatus.NoData);
                }
                var accountingInf = GetAccountingInfAllRaiinNo(syunoSeikyu);

                //Get GetPtByoMeiList
                var ptByoMei = _accountingRepository.GetPtByoMeiList(inputData.HpId, inputData.PtId, inputData.SinDate);

                return new GetAccountingOutputData(syunoSeikyu, accountingInf, ptByoMei, GetAccountingStatus.Successed);

            }
            catch (Exception)
            {
                return new GetAccountingOutputData(new List<AccountingModel>(), new AccountingInfModel(), new List<PtByomeiModel>(), GetAccountingStatus.Failed);
            }
            finally
            {
                _accountingRepository.ReleaseResource();
            }
        }

        public AccountingInfModel GetAccountingInfAllRaiinNo(List<AccountingModel> accountingModels)
        {
            try
            {
                var isSettled = accountingModels.Select(item => item.SyunoSeikyu.NyukinKbn != 0).FirstOrDefault();

                var TotalPoint = accountingModels.Sum(item => item.SyunoSeikyu.SeikyuTensu);

                var KanFutan = accountingModels.Sum(item => item.PtFutan + item.AdjustRound);

                var TotalSelfExpense =
                    accountingModels.Sum(item => item.JihiFutan + item.JihiOuttax);
                var Tax =
                    accountingModels.Sum(item => item.JihiTax + item.JihiOuttax);
                var AdjustFutan = accountingModels.Sum(item => item.AdjFutan);

                var DebitBalance = accountingModels.Sum(item => item.SyunoSeikyu.SeikyuGaku -
                                                      item.SyunoNyukinModels.Sum(itemNyukin =>
                                                          itemNyukin.NyukinGaku + itemNyukin.AdjustFutan));

                var SumAdjust = 0;
                var SumAdjustView = 0;
                var ThisCredit = 0;
                var ThisWari = 0;
                var PayType = 0;
                if (isSettled == true)
                {
                    SumAdjust = accountingModels.Sum(item => item.SyunoSeikyu.SeikyuGaku);
                    SumAdjustView = SumAdjust;
                    ThisCredit =
                       accountingModels.Sum(item => item.SyunoNyukinModels.Sum(itemNyukin => itemNyukin.NyukinGaku));
                    ThisWari =
                       accountingModels.Sum(item => item.SyunoNyukinModels.Sum(itemNyukin => itemNyukin.AdjustFutan));
                    PayType = accountingModels.Where(item => item.SyunoNyukinModels.Count > 0)
                       .Select(item => item.SyunoNyukinModels.Where(itemNyukin => itemNyukin.PaymentMethodCd > 0)
                           .Select(itemNyukin => itemNyukin.PaymentMethodCd).FirstOrDefault())
                       .FirstOrDefault();
                }
                else
                {
                    SumAdjust = accountingModels.Sum(item => item.SyunoSeikyu.SeikyuGaku);
                    SumAdjustView = SumAdjust + DebitBalance;

                    ThisCredit = SumAdjust;
                }
                return new AccountingInfModel(TotalPoint, KanFutan, TotalSelfExpense, Tax, DebitBalance, SumAdjust, SumAdjustView, ThisCredit, ThisWari, PayType, AdjustFutan);
            }
            catch (Exception)
            {

                throw;
            }

        }



    }
}
