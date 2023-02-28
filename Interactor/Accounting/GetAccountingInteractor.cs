using Domain.Models.AccountDue;
using Domain.Models.Accounting;
using Domain.Models.Insurance;
using Domain.Models.Reception;
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
                    return new GetAccountingOutputData(new(), GetAccountingStatus.NoData, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, new());
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

                var listKohi = GetVisibilityPtKohiModelList(listRaiinInf, inputData.HpId, inputData.PtId, inputData.SinDate);

                return GetAccountingInf(listKohi, listSyunoSeikyu, debitBalance, checkDebitBalance);
            }
            finally
            {
                _accountingRepository.ReleaseResource();
                _systemConfRepository.ReleaseResource();
            }
        }

        private List<KohiInfModel> GetVisibilityPtKohiModelList(List<ReceptionDto> receptionDtos, int hpId, long ptId, int sinDate)
        {
            var kohiIds = new List<int>();
            var listKaikeiInfAll = receptionDtos.Where(item => item.RaiinNo > 0 && item.KaikeiInfModels != null).ToList();
            var listKaikeiInf = new List<KaikeiInfModel>();

            foreach (var kaikeiInf in listKaikeiInfAll)
            {
                listKaikeiInf.AddRange(kaikeiInf.KaikeiInfModels);
            }

            foreach (var kaikeiInf in listKaikeiInf)
            {
                if (kaikeiInf.Kohi1Id > 0 &&
                    !kohiIds.Contains(kaikeiInf.Kohi1Id))
                {
                    kohiIds.Add(kaikeiInf.Kohi1Id);
                }

                if (kaikeiInf.Kohi2Id > 0 &&
                    !kohiIds.Contains(kaikeiInf.Kohi2Id))
                {
                    kohiIds.Add(kaikeiInf.Kohi2Id);
                }

                if (kaikeiInf.Kohi3Id > 0 &&
                    !kohiIds.Contains(kaikeiInf.Kohi3Id))
                {
                    kohiIds.Add(kaikeiInf.Kohi3Id);
                }

                if (kaikeiInf.Kohi4Id > 0 &&
                    !kohiIds.Contains(kaikeiInf.Kohi4Id))
                {
                    kohiIds.Add(kaikeiInf.Kohi4Id);
                }
            }

            if (kohiIds.Count <= 0) return new();

            var listKohi = _accountingRepository.GetListKohiByKohiId(hpId, ptId, sinDate, kohiIds);

            return listKohi.Where(item => item.HokenMstModel != null && (item.HokenMstModel.MoneyLimitListFlag != 0 || item.HokenMstModel.MonthLimitCount > 0)).ToList();
        }

        private GetAccountingOutputData GetAccountingInf(List<KohiInfModel> kohiInfModels, List<SyunoSeikyuModel> listSyunoSeikyu, int debitBalance, bool checkDebitBalance)
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

            return new GetAccountingOutputData(listSyunoSeikyu, GetAccountingStatus.Successed, totalPoint, kanFutan, totalSelfExpense, tax, adjustFutan, debitBalance, sumAdjust, sumAdjustView, thisCredit, thisWari, kohiInfModels);
        }
    }
}
