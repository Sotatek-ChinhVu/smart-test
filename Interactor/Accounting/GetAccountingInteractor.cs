using Domain.Models.Accounting;
using Domain.Models.HokenMst;
using Domain.Models.Insurance;
using Helper.Common;
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
                //Get Raiin Inf
                var raiinInf = _accountingRepository.GetListRaiinInf(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.RaiinNo).ToList();

                //Get Accounting Inf
                var syunoSeikyu = _accountingRepository.GetListSyunoSeikyu(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.RaiinNo).ToList();

                if (!syunoSeikyu.Any())
                {
                    return new GetAccountingOutputData(new List<RaiinInfModel>(), new List<AccountingModel>(), new AccountingInfModel(), new List<WarningMemoModel>(), new List<PtByomeiModel>(), GetAccountingStatus.NoData);
                }
                var accountingInf = GetAccountingInfAllRaiinNo(syunoSeikyu);

                //Get Warning Memo
                var warning = GetWarningMemo(inputData.HpId, inputData.PtId, inputData.SinDate, raiinInf);

                //Get GetPtByoMeiList
                var ptByoMei = _accountingRepository.GetPtByoMeiList(inputData.HpId, inputData.PtId, inputData.SinDate);
                return new GetAccountingOutputData(raiinInf, syunoSeikyu, accountingInf, warning, ptByoMei, GetAccountingStatus.Successed);

            }
            catch (Exception)
            {
                return new GetAccountingOutputData(new List<RaiinInfModel>(), new List<AccountingModel>(), new AccountingInfModel(), new List<WarningMemoModel>(), new List<PtByomeiModel>(), GetAccountingStatus.Failed);
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

        private List<WarningMemoModel> GetWarningMemo(int hpId, long ptId, int sinDate, List<RaiinInfModel> raiinInfModels)
        {
            List<WarningMemoModel> WarningMemoModel = new List<WarningMemoModel>();
            bool CheckIdIsExits(List<int> idList, int idCheck)
            {
                if (idList != null)
                {
                    return idList.Contains(idCheck);
                }
                return false;
            }

            List<int> listKohiId = new List<int>();

            foreach (var raiin in raiinInfModels)
            {
                if (raiin.HokenPatternModel == null) continue;

                if (raiin.HokenPatternModel.PtHokenInfModel != null &&
                    raiin.HokenPatternModel.PtHokenInfModel.HokenId > 0)
                {
                    WarningMemoModel.Add(CheckHokenIsExpirated(raiin.RaiinNo, raiin.HokenPatternModel.PtHokenInfModel));
                    WarningMemoModel.Add(CheckHokenHasDateConfirmed(raiin.RaiinNo, raiin.HokenPatternModel.PtHokenInfModel));
                }

                if (raiin.HokenPatternModel.Kohi1 != null &&
                    raiin.HokenPatternModel.Kohi1.HokenId > 0 &&
                    !CheckIdIsExits(listKohiId, raiin.HokenPatternModel.Kohi1.HokenId))
                {
                    listKohiId.Add(raiin.HokenPatternModel.Kohi1.HokenId);
                    WarningMemoModel.Add(CheckKohiIsExpirated(raiin.RaiinNo, raiin.HokenPatternModel.Kohi1));
                    WarningMemoModel.Add(CheckKohiHasDateConfirmed(raiin.RaiinNo, raiin.HokenPatternModel.Kohi1));
                }

                if (raiin.HokenPatternModel.Kohi2 != null &&
                    raiin.HokenPatternModel.Kohi2.HokenId > 0 &&
                    !CheckIdIsExits(listKohiId, raiin.HokenPatternModel.Kohi2.HokenId))
                {
                    listKohiId.Add(raiin.HokenPatternModel.Kohi2.HokenId);
                    WarningMemoModel.Add(CheckKohiIsExpirated(raiin.RaiinNo, raiin.HokenPatternModel.Kohi2));
                    WarningMemoModel.Add(CheckKohiHasDateConfirmed(raiin.RaiinNo, raiin.HokenPatternModel.Kohi2));
                }

                if (raiin.HokenPatternModel.Kohi3 != null &&
                    raiin.HokenPatternModel.Kohi3.HokenId > 0 &&
                    !CheckIdIsExits(listKohiId, raiin.HokenPatternModel.Kohi3.HokenId))
                {
                    listKohiId.Add(raiin.HokenPatternModel.Kohi3.HokenId);
                    WarningMemoModel.Add(CheckKohiIsExpirated(raiin.RaiinNo, raiin.HokenPatternModel.Kohi3));
                    WarningMemoModel.Add(CheckKohiHasDateConfirmed(raiin.RaiinNo, raiin.HokenPatternModel.Kohi3));
                }

                if (raiin.HokenPatternModel.Kohi4 != null &&
                    raiin.HokenPatternModel.Kohi4.HokenId > 0 &&
                    !CheckIdIsExits(listKohiId, raiin.HokenPatternModel.Kohi4.HokenId))
                {
                    listKohiId.Add(raiin.HokenPatternModel.Kohi4.HokenId);
                    WarningMemoModel.Add(CheckKohiIsExpirated(raiin.RaiinNo, raiin.HokenPatternModel.Kohi4));
                    WarningMemoModel.Add(CheckKohiHasDateConfirmed(raiin.RaiinNo, raiin.HokenPatternModel.Kohi4));
                }
            }

            var listCalcLog = _accountingRepository.GetCalcLog(hpId, ptId, sinDate, raiinInfModels.Select(x => x.RaiinNo).ToList());

            if (listCalcLog != null && listCalcLog.Count > 0)
            {
                foreach (var calcLog in listCalcLog)
                {
                    WarningMemoModel.Add(new WarningMemoModel(calcLog.RaiinNo, calcLog.Text ?? string.Empty, calcLog.LogSbt));
                }
            }
            return WarningMemoModel;
        }
        private WarningMemoModel CheckHokenIsExpirated(long raiinNo, PtHokenInfModel ptHokenInf, int alertFg = 1)
        {
            if (ptHokenInf == null || ptHokenInf.IsReceKisaiOrNoHoken) return new();

            if (ptHokenInf.IsExpirated == true)
            {
                if (ptHokenInf.IsHoken)
                {
                    return new WarningMemoModel(raiinNo, string.Format(
                        $"【保険確認】 主保険の有効期限が切れています。（{CIUtil.SDateToShowWDate(ptHokenInf.StartDate)} ～ {CIUtil.SDateToShowWDate(ptHokenInf.EndDate)}）"),
                        alertFg);
                }
                else if (ptHokenInf.IsRousai)
                {
                    return new WarningMemoModel(raiinNo, string.Format(
                        $"【保険確認】 労災保険の有効期限が切れています。（{CIUtil.SDateToShowWDate(ptHokenInf.StartDate)} ～ {CIUtil.SDateToShowWDate(ptHokenInf.EndDate)}）"),
                        alertFg);
                }
                else if (ptHokenInf.IsJibai)
                {
                    return new WarningMemoModel(raiinNo, string.Format(
                        $"【保険確認】 自賠責保険の有効期限が切れています。（{CIUtil.SDateToShowWDate(ptHokenInf.StartDate)} ～ {CIUtil.SDateToShowWDate(ptHokenInf.EndDate)}）"),
                        alertFg);
                }
            }
            return new();
        }

        private WarningMemoModel CheckHokenHasDateConfirmed(long raiinNo, PtHokenInfModel ptHokenInf, int alertFg = 0)
        {
            if (ptHokenInf == null || ptHokenInf.IsReceKisaiOrNoHoken) return new();

            if (ptHokenInf.HasDateConfirmed == false)
            {
                if (ptHokenInf.IsHoken)
                {
                    return new WarningMemoModel(raiinNo,
                        string.Format(
                            $"【保険確認】 保険が未確認です。（最終確認日：{CIUtil.SDateToShowWDate(ptHokenInf.LastDateConfirmed)}）"),
                        alertFg);
                }
            }
            return new();
        }

        private WarningMemoModel CheckKohiIsExpirated(long raiinNo, KohiInfModel ptKohiModel, int alertFg = 1)
        {
            if (ptKohiModel == null) return new();
            if (ptKohiModel.IsExpirated == true)
            {
                if (!string.IsNullOrWhiteSpace(ptKohiModel.FutansyaNo))
                {
                    return new WarningMemoModel(raiinNo,
                        string.Format(
                            $"【保険確認】 公費（{ptKohiModel.FutansyaNo}）の有効期限が切れています。（{CIUtil.SDateToShowWDate(ptKohiModel.StartDate)} ～ {CIUtil.SDateToShowWDate(ptKohiModel.EndDate)}）"),
                        alertFg);
                }
                else
                {
                    return new WarningMemoModel(raiinNo,
                        string.Format(
                            $"【保険確認】 公費 の有効期限が切れています。（{CIUtil.SDateToShowWDate(ptKohiModel.StartDate)} ～ {CIUtil.SDateToShowWDate(ptKohiModel.EndDate)}）"),
                        alertFg);
                }
            }
            return new();
        }

        private WarningMemoModel CheckKohiHasDateConfirmed(long raiinNo, KohiInfModel ptKohiModel, int alertFg = 0)
        {
            if (ptKohiModel == null) return new();
            if (ptKohiModel.HasDateConfirmed == false)
            {
                if (!string.IsNullOrWhiteSpace(ptKohiModel.FutansyaNo))
                {
                    return new WarningMemoModel(raiinNo, string.Format(
                            $"【保険確認】 公費（{ptKohiModel.FutansyaNo}）の保険証が未確認です。（最終確認日：{CIUtil.SDateToShowWDate(ptKohiModel.LastDateConfirmed)}）"),
                        alertFg);
                }
                else
                {
                    return new WarningMemoModel(raiinNo,
                        string.Format(
                            $"【保険確認】 公費 の保険証が未確認です。（最終確認日：{CIUtil.SDateToShowWDate(ptKohiModel.LastDateConfirmed)}）"),
                        alertFg);
                }
            }
            return new();
        }

    }
}
