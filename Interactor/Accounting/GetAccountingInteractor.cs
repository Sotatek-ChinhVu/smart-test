using Domain.Models.AccountDue;
using Domain.Models.Accounting;
using Domain.Models.SystemConf;
using UseCase.Accounting.GetAccountingInf;

namespace Interactor.Accounting
{
    public class GetAccountingInteractor : IGetAccountingInputPort
    {
        private readonly IAccountingRepository _accountingRepository;
        private readonly ISystemConfRepository _systemConfRepository;

        public GetAccountingInteractor(IAccountingRepository accountingRepository, ISystemConfRepository systemConfRepository)
        {
            _accountingRepository = accountingRepository;
            _systemConfRepository = systemConfRepository;
        }

        public GetAccountingOutputData Handle(GetAccountingInputData inputData)
        {
            try
            {
                var listRaiinInf = _accountingRepository.GetListRaiinInf(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.RaiinNo);

                var listRaiinNo = listRaiinInf.Select(r => r.RaiinNo).ToList();

                var listSyunoSeikyu = _accountingRepository.GetListSyunoSeikyu(inputData.HpId, inputData.PtId, inputData.SinDate, listRaiinNo);

                var syunoSeikyu = listSyunoSeikyu.FirstOrDefault(x => x.RaiinNo == inputData.RaiinNo);

                if (syunoSeikyu == null)
                {
                    return new GetAccountingOutputData(new List<SyunoSeikyuModel>(), GetAccountingStatus.NoData, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                }
                else if (syunoSeikyu.NyukinKbn == 0)
                {
                    listSyunoSeikyu = listSyunoSeikyu.Where(item => item.NyukinKbn == 0).ToList();
                }
                else
                {
                    listSyunoSeikyu = listSyunoSeikyu.Where(item => item.NyukinKbn != 0).ToList();
                }

                var listAllSyunoSeikyu = _accountingRepository.GetListSyunoSeikyu(inputData.HpId, inputData.PtId, inputData.SinDate, listRaiinNo, true);

                var debitBalance = listAllSyunoSeikyu.Sum(item => item.SeikyuGaku -
                                                  item.SyunoNyukinModels.Sum(itemNyukin =>
                                                      itemNyukin.NyukinGaku + itemNyukin.AdjustFutan));
                var checkDebitBalance = (int)_systemConfRepository.GetSettingValue(3020, 0, 0) == 1;

                return GetAccountingInf(listSyunoSeikyu, debitBalance, checkDebitBalance);
            }
            catch (Exception)
            {
                return new GetAccountingOutputData(new List<SyunoSeikyuModel>(), GetAccountingStatus.Failed, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            }
            finally
            {
                _accountingRepository.ReleaseResource();
                _systemConfRepository.ReleaseResource();
            }
        }

        private GetAccountingOutputData GetAccountingInf(List<SyunoSeikyuModel> listSyunoSeikyu, int debitBalance, bool checkDebitBalance)
        {
            var isSettled = listSyunoSeikyu.Select(item => item.NyukinKbn != 0).FirstOrDefault();

            var totalPoint = listSyunoSeikyu.Sum(item => item.SeikyuTensu);
            var kanFutan = listSyunoSeikyu.SelectMany(x => x.KaikeiInfModels).Sum(item => item.PtFutan + item.AdjustRound);

            var totalSelfExpense =
                listSyunoSeikyu.SelectMany(x => x.KaikeiInfModels).Sum(item => item.JihiFutan + item.JihiOuttax);
            var tax =
                listSyunoSeikyu.SelectMany(x => x.KaikeiInfModels).Sum(item => item.JihiTax + item.JihiOuttax);
            var adjustFutan = listSyunoSeikyu.Sum(item => item.AdjustFutan);

            var sumAdjust = 0;
            var sumAdjustView = 0;
            var thisCredit = 0;
            var thisWari = 0;
            if (isSettled)
            {
                sumAdjust = listSyunoSeikyu.Sum(item => item.SeikyuGaku);
                sumAdjustView = sumAdjust;
                thisCredit =
                   listSyunoSeikyu.SelectMany(x => x.SyunoNyukinModels).Sum(itemNyukin => itemNyukin.NyukinGaku);
                thisWari =
                   listSyunoSeikyu.SelectMany(x => x.SyunoNyukinModels).Sum(itemNyukin => itemNyukin.AdjustFutan);
                var payType = listSyunoSeikyu.Where(item => item.SyunoNyukinModels.Count > 0)
                    .Select(item => item.SyunoNyukinModels.Where(itemNyukin => itemNyukin.PaymentMethodCd > 0)
                        .Select(itemNyukin => itemNyukin.PaymentMethodCd).FirstOrDefault())
                    .FirstOrDefault();
            }
            else
            {
                sumAdjust = listSyunoSeikyu.Sum(item => item.SeikyuGaku);
                sumAdjustView = sumAdjust + debitBalance;
                if (!checkDebitBalance)
                {
                    sumAdjust = sumAdjustView;
                }
                thisCredit = sumAdjust;
            }

            return new GetAccountingOutputData(listSyunoSeikyu, GetAccountingStatus.Successed, totalPoint, kanFutan, totalSelfExpense, tax, adjustFutan, debitBalance, sumAdjust, sumAdjustView, thisCredit, thisWari);
        }
    }
}
