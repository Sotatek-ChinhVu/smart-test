using Domain.Models.AccountDue;
using Domain.Models.Accounting;
using Domain.Models.SystemConf;
using Helper.Common;
using UseCase.Accounting.CheckAccountingStatus;

namespace Interactor.Accounting
{
    public class CheckAccountingStatusInteractor : ICheckAccountingStatusInputPort
    {
        private readonly IAccountingRepository _accountingRepository;
        private readonly ISystemConfRepository _systemConfRepository;

        public CheckAccountingStatusInteractor(IAccountingRepository accountingRepository, ISystemConfRepository systemConfRepository)
        {
            _accountingRepository = accountingRepository;
            _systemConfRepository = systemConfRepository;
        }

        public CheckAccountingStatusOutputData Handle(CheckAccountingStatusInputData inputData)
        {
            try
            {
                var billUpdate = "今回請求額が更新されています。\n窓口精算画面を開きなおしてください。";
                var validCredit = "今回入金額\nを入力してください。";
                var verifyDate = "請求金額が変更されているため、以下の請求日の領収証を印刷できません";
                var validAmount = "入金額が正しくありません。・入金額を確認し、再実行してください。";
                var mbOk = "mbOk";
                var mbClose = "mbClose";
                var yesNoCancel = "YesNoCancel";

                var raiinNos = _accountingRepository.GetRaiinNos(inputData.HpId, inputData.PtId, inputData.RaiinNo);

                var syunoSeikyusChecking = _accountingRepository.GetListSyunoSeikyu(inputData.HpId, inputData.PtId, inputData.SinDate, raiinNos);

                var allSyunoSeikyusChecking = _accountingRepository.GetListSyunoSeikyu(inputData.HpId, inputData.PtId, inputData.SinDate, raiinNos, true);

                //CheckAccounting when is DisChargeClick
                if (inputData.IsDisCharge)
                {
                    if (syunoSeikyusChecking == null)
                    {
                        return new CheckAccountingStatusOutputData(mbOk, billUpdate, CheckAccountingStatus.BillUpdated);
                    }

                    var syunoChanged = CheckSyunoChanged(inputData.SyunoSeikyuDtos, inputData.AllSyunoSeikyuDtos, syunoSeikyusChecking, allSyunoSeikyusChecking);
                    if (syunoChanged)
                    {
                        return new CheckAccountingStatusOutputData(mbOk, billUpdate, CheckAccountingStatus.BillUpdated);
                    }

                    var accDue = 0;
                    var setting = _systemConfRepository.GetSettingValue(3020, 0, 0) == 1;
                    if (!setting)
                    {
                        accDue = inputData.DebitBalance;
                    }

                    if (!CheckCredit(accDue, inputData.SumAdjust, inputData.ThisCredit, inputData.Wari, true))
                    {
                        return new CheckAccountingStatusOutputData(mbClose, validAmount, CheckAccountingStatus.ValidPaymentAmount);
                    }

                    var dateNotVerify = VerifyCredit(accDue, inputData.SumAdjust, inputData.ThisCredit, inputData.Wari, syunoSeikyusChecking, allSyunoSeikyusChecking, inputData.IsDisCharge);
                    if (!string.IsNullOrEmpty(dateNotVerify))
                    {
                        var mess = $"{dateNotVerify}{Environment.NewLine}{Environment.NewLine}収納一覧を開いて、請求金額を変更しますか？";
                        return new CheckAccountingStatusOutputData(yesNoCancel, string.Concat(mess, verifyDate), CheckAccountingStatus.DateNotVerify);
                    }
                }

                //CheckAccounting when save
                if (inputData.IsSaveAccounting)
                {
                    if (syunoSeikyusChecking == null)
                    {
                        return new CheckAccountingStatusOutputData(mbOk, billUpdate, CheckAccountingStatus.BillUpdated);
                    }

                    var syunoChanged = CheckSyunoChanged(inputData.SyunoSeikyuDtos, inputData.AllSyunoSeikyuDtos, syunoSeikyusChecking, allSyunoSeikyusChecking);

                    if (syunoChanged)
                    {
                        return new CheckAccountingStatusOutputData(mbOk, billUpdate, CheckAccountingStatus.BillUpdated);
                    }

                    if (inputData.ThisCredit < 0)
                    {
                        return new CheckAccountingStatusOutputData(mbClose, validCredit, CheckAccountingStatus.ValidThisCredit);
                    }

                    var accDue = 0;
                    var setting = _systemConfRepository.GetSettingValue(3020, 0, 0) == 1;
                    if (!CheckCredit(accDue, inputData.SumAdjust, inputData.ThisCredit, inputData.Wari))
                    {
                        return new CheckAccountingStatusOutputData(mbClose, validAmount, CheckAccountingStatus.ValidPaymentAmount);
                    }

                    var dateNotVerify = VerifyCredit(accDue, inputData.SumAdjust, inputData.ThisCredit, inputData.Wari, syunoSeikyusChecking, allSyunoSeikyusChecking);
                    if (!string.IsNullOrEmpty(dateNotVerify))
                    {

                        var mess = $"{dateNotVerify}{Environment.NewLine}{Environment.NewLine}収納一覧を開いて、請求金額を変更しますか？";
                        return new CheckAccountingStatusOutputData(yesNoCancel, string.Concat(mess, verifyDate), CheckAccountingStatus.DateNotVerify);
                    }
                }

                return new CheckAccountingStatusOutputData(string.Empty, string.Empty, CheckAccountingStatus.Successed);
            }
            finally
            {
                _accountingRepository.ReleaseResource();
                _systemConfRepository.ReleaseResource();
            }
        }

        private bool CheckSyunoChanged(List<SyunoSeikyuDto> currentList, List<SyunoSeikyuDto> allCurrentSyuno, List<SyunoSeikyuModel> syunoSeikyusChecking, List<SyunoSeikyuModel> allSyunoSeikyusChecking)
        {

            if (CheckSyunoDifferent(currentList, syunoSeikyusChecking))
            {
                return true;
            }

            if (CheckSyunoDifferent(allCurrentSyuno, allSyunoSeikyusChecking))
            {
                return true;
            }

            return false;
        }

        private bool CheckSyunoDifferent(List<SyunoSeikyuDto> syunoSeikyusOld, List<SyunoSeikyuModel> syunoSeikyusNew)
        {
            if (syunoSeikyusOld.Count != syunoSeikyusNew.Count)
            {
                return true;
            }

            foreach (var seikyuOld in syunoSeikyusOld)
            {
                var seikyuNew = syunoSeikyusNew.FirstOrDefault(item => item.RaiinNo == seikyuOld.RaiinNo);
                if (seikyuNew == null)
                {
                    return true;
                }

                if (seikyuOld.NyukinKbn != seikyuNew.NyukinKbn ||
                    seikyuOld.SeikyuGaku != seikyuNew.SeikyuGaku ||
                    seikyuOld.NewSeikyuGaku != seikyuNew.NewSeikyuGaku)
                {
                    return true;
                }

                if (seikyuOld.SyunoNyukinModels.Sum(itemNyukin => itemNyukin.NyukinGaku + itemNyukin.AdjustFutan) !=
                    seikyuNew.SyunoNyukinModels.Sum(itemNyukin => itemNyukin.NyukinGaku + itemNyukin.AdjustFutan))
                {
                    return true;
                }
            }

            return false;
        }

        private bool CheckCredit(int accDue, int sumAdjust, int credit, int wari, bool isDisCharge = false)
        {
            bool result = false;
            if (isDisCharge == true)
            {
                if (accDue == 0 || credit == 0)
                {
                    result = true;
                }
                else if (accDue < 0)
                {
                    if (accDue == credit)
                    {
                        result = true;
                    }
                }
                else
                {
                    if (credit >= 0 && credit <= accDue)
                    {
                        result = true;
                    }
                }
            }
            else
            {
                if (accDue < 0)
                {
                    int sumCredit = credit + wari;
                    if (sumCredit == 0)
                    {
                        result = true;
                    }
                    else if (sumCredit <= sumAdjust)
                    {
                        result = true;
                    }
                }
                else
                {
                    if (credit + wari <= sumAdjust)
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        private string VerifyCredit(int accDue, int sumAdjust, int credit, int wari, List<SyunoSeikyuModel> listSyunoSeikyu,
            List<SyunoSeikyuModel> listAllSyunoSeikyu, bool isDisCharge = false)
        {
            string dateNotVerify = "";

            int allSeikyuGaku = sumAdjust;
            int adjustFutan = wari;
            int nyukinGaku = credit;
            int outAdjustFutan = 0;
            int outNyukinGaku = 0;
            int outNyukinKbn = 0;

            if (isDisCharge == false)
            {
                for (int i = 0; i < listSyunoSeikyu.Count; i++)
                {
                    var item = listSyunoSeikyu[i];
                    int thisSeikyuGaku = item.SeikyuGaku - item.SyunoNyukinModels.Sum(itemNyukin => itemNyukin.NyukinGaku) -
                                     item.SyunoNyukinModels.Sum(itemNyukin => itemNyukin.AdjustFutan);
                    bool isLastRecord = i == listSyunoSeikyu.Count - 1;

                    ParseValueUpdate(allSeikyuGaku, thisSeikyuGaku, ref adjustFutan, ref nyukinGaku, out outAdjustFutan, out outNyukinGaku,
                        out outNyukinKbn, isLastRecord);

                    allSeikyuGaku -= thisSeikyuGaku;
                }
            }

            if (accDue != 0 && nyukinGaku != 0)
            {
                bool isSettled = nyukinGaku == accDue;

                foreach (var item in listAllSyunoSeikyu)
                {
                    if (nyukinGaku == 0) break;

                    int thisSeikyuGaku = item.SeikyuGaku - item.SyunoNyukinModels.Sum(itemNyukin => itemNyukin.NyukinGaku) -
                                         item.SyunoNyukinModels.Sum(itemNyukin => itemNyukin.AdjustFutan);

                    ParseEarmarkedValueUpdate(thisSeikyuGaku, ref nyukinGaku, out outNyukinGaku,
                        out outNyukinKbn, isSettled);

                    if (item.SeikyuGaku != item.NewSeikyuGaku && item.NewSeikyuGaku > 0)
                    {
                        dateNotVerify = GetAllDateNotVerify(listAllSyunoSeikyu);
                        break;
                    }
                }
            }

            return dateNotVerify;
        }

        private string GetAllDateNotVerify(List<SyunoSeikyuModel> listAllSyunoSeikyu)
        {
            string dateNotVerify = "";

            var listNotVerify = listAllSyunoSeikyu
                .Where(item => item.SeikyuGaku != item.NewSeikyuGaku && item.NewSeikyuGaku > 0)
                .OrderBy(item => item.SinDate)
                .ThenBy(item => item.RaiinNo)
                .ToList();

            if (listNotVerify.Count > 0)
            {
                foreach (var item in listNotVerify)
                {
                    dateNotVerify = $"{dateNotVerify}{Environment.NewLine}{CIUtil.SDateToShowSDate(item.SinDate)}";
                }
            }

            return dateNotVerify;
        }

        private void ParseValueUpdate(int allSeikyuGaku, int thisSeikyuGaku, ref int adjustFutan, ref int nyukinGaku, out int outAdjustFutan,
            out int outNyukinGaku, out int outNyukinKbn, bool isLastRecord)
        {
            int credit = adjustFutan + nyukinGaku;

            if (credit == allSeikyuGaku || credit < allSeikyuGaku && credit > thisSeikyuGaku)
            {
                if (isLastRecord == true)
                {
                    outAdjustFutan = adjustFutan;
                    outNyukinGaku = thisSeikyuGaku - outAdjustFutan;

                    adjustFutan -= outAdjustFutan;
                    nyukinGaku -= outNyukinGaku;
                }
                else if (adjustFutan >= thisSeikyuGaku)
                {
                    outAdjustFutan = thisSeikyuGaku;
                    outNyukinGaku = thisSeikyuGaku - outAdjustFutan;

                    adjustFutan -= outAdjustFutan;
                    nyukinGaku -= outNyukinGaku;
                }
                else
                {
                    outAdjustFutan = adjustFutan;
                    outNyukinGaku = thisSeikyuGaku - outAdjustFutan;

                    adjustFutan -= outAdjustFutan;
                    nyukinGaku -= outNyukinGaku;
                }
            }
            else
            {
                outAdjustFutan = adjustFutan;
                outNyukinGaku = nyukinGaku;

                adjustFutan -= outAdjustFutan;
                nyukinGaku -= outNyukinGaku;
            }

            thisSeikyuGaku = thisSeikyuGaku - outAdjustFutan - outNyukinGaku;
            outNyukinKbn = thisSeikyuGaku == 0 ? 3 : 1;
        }

        private void ParseEarmarkedValueUpdate(int thisSeikyuGaku, ref int nyukinGaku, out int outNyukinGaku,
            out int outNyukinKbn, bool isSettled = false)
        {
            if (isSettled == true)
            {
                outNyukinGaku = thisSeikyuGaku;
                nyukinGaku -= outNyukinGaku;
                outNyukinKbn = 3;
                return;
            }

            if (nyukinGaku >= thisSeikyuGaku)
            {
                outNyukinGaku = thisSeikyuGaku;
                nyukinGaku -= outNyukinGaku;
            }
            else
            {
                outNyukinGaku = nyukinGaku;
                nyukinGaku -= outNyukinGaku;
            }

            thisSeikyuGaku -= outNyukinGaku;
            outNyukinKbn = thisSeikyuGaku == 0 ? 3 : 1;
        }
    }
}
