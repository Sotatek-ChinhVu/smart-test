using Domain.Models.Accounting;
using Domain.Models.HokenMst;
using Domain.Models.Insurance;
using Helper.Common;
using UseCase.Accounting.GetWarningMemo;

namespace Interactor.Accounting
{
    public class GetWarningMemoInteractor : IGetWarningMemoInputPort
    {
        private readonly IHokenMstRepository _hokenMstRepository;

        public GetWarningMemoInteractor(IHokenMstRepository hokenMstRepository)
        {
            _hokenMstRepository = hokenMstRepository;
        }

        public GetWarningMemoOutputData Handle(GetWarningMemoInputData inputData)
        {
            throw new NotImplementedException();
        }

        private void GetWarningMemo(int hpId, long ptId, int sinDate, long raiinNo)
        {
            bool CheckIdIsExits(List<int> idList, int idCheck)
            {
                if (idList != null)
                {
                    return idList.Contains(idCheck);
                }
                return false;
            }


            string warningMemo = string.Empty;

            List<int> listKohiId = new List<int>();
            var listHokenPattern = _hokenMstRepository.FindPtHokenPatternList(hpId, ptId, sinDate, raiinNo);

            foreach (var item in listHokenPattern)
            {
                if (item != null && item.HokenId > 0)
                {
                    CheckHokenIsExpirated(item.HokenInfModel);
                    CheckHokenHasDateConfirmed(item.HokenInfModel);
                }

                if (item.Kohi1InfModel != null &&
                    item.PtHokenPatternModel.Kohi1Inf.HokenId > 0 &&
                    !CheckIdIsExits(listKohiId, item.PtHokenPatternModel.Kohi1Inf.HokenId))
                {
                    listKohiId.Add(item.PtHokenPatternModel.Kohi1Inf.HokenId);
                    CheckKohiIsExpirated(ref richTextBox, item.PtHokenPatternModel.Kohi1Inf);
                    CheckKohiHasDateConfirmed(ref richTextBox, item.PtHokenPatternModel.Kohi1Inf);
                }

                if (item.PtHokenPatternModel.Kohi2Inf != null &&
                    item.PtHokenPatternModel.Kohi2Inf.HokenId > 0 &&
                    !CheckIdIsExits(listKohiId, item.PtHokenPatternModel.Kohi2Inf.HokenId))
                {
                    listKohiId.Add(item.PtHokenPatternModel.Kohi2Inf.HokenId);
                    CheckKohiIsExpirated(ref richTextBox, item.PtHokenPatternModel.Kohi2Inf);
                    CheckKohiHasDateConfirmed(ref richTextBox, item.PtHokenPatternModel.Kohi2Inf);
                }

                if (item.PtHokenPatternModel.Kohi3Inf != null &&
                    item.PtHokenPatternModel.Kohi3Inf.HokenId > 0 &&
                    !CheckIdIsExits(listKohiId, item.PtHokenPatternModel.Kohi3Inf.HokenId))
                {
                    listKohiId.Add(item.PtHokenPatternModel.Kohi3Inf.HokenId);
                    CheckKohiIsExpirated(ref richTextBox, item.PtHokenPatternModel.Kohi3Inf);
                    CheckKohiHasDateConfirmed(ref richTextBox, item.PtHokenPatternModel.Kohi3Inf);
                }

                if (item.PtHokenPatternModel.Kohi4Inf != null &&
                    item.PtHokenPatternModel.Kohi4Inf.HokenId > 0 &&
                    !CheckIdIsExits(listKohiId, item.PtHokenPatternModel.Kohi4Inf.HokenId))
                {
                    listKohiId.Add(item.PtHokenPatternModel.Kohi4Inf.HokenId);
                    CheckKohiIsExpirated(ref richTextBox, item.PtHokenPatternModel.Kohi4Inf);
                    CheckKohiHasDateConfirmed(ref richTextBox, item.PtHokenPatternModel.Kohi4Inf);
                }
            }

            var listCalcLog = _accountingFinder.GetCalcLog(PtId, SinDate, listRaiinNo);

            if (listCalcLog != null && listCalcLog.Count > 0)
            {
                foreach (var calcLog in listCalcLog)
                {
                    AddWarning(ref richTextBox, calcLog.Text, calcLog.LogSbt);
                }
            }

            TextWarningMemo = richTextBox.Text;
            RtfWarningMemo = richTextBox.Rtf;

            IsLoadSimpleWarningMemo = true;
        }

        private WarningMemoModel CheckHokenIsExpirated(HokenInfModel hokenInf, int alertFg = 1)
        {
            if (hokenInf == null || hokenInf.IsReceKisaiOrNoHoken) return new WarningMemoModel();

            if (hokenInf.IsExpirated == true)
            {
                if (hokenInf.IsHoken)
                {
                    return AddWarning(string.Format(
                         $"【保険確認】 主保険の有効期限が切れています。（{CIUtil.SDateToShowWDate(hokenInf.StartDate)} ～ {CIUtil.SDateToShowWDate(hokenInf.EndDate)}）"),
                         alertFg);
                }
                else if (hokenInf.IsRousai)
                {
                    return AddWarning(string.Format(
                        $"【保険確認】 労災保険の有効期限が切れています。（{CIUtil.SDateToShowWDate(hokenInf.StartDate)} ～ {CIUtil.SDateToShowWDate(hokenInf.EndDate)}）"),
                        alertFg);
                }
                else if (hokenInf.IsJibai)
                {
                    return AddWarning(string.Format(
                        $"【保険確認】 自賠責保険の有効期限が切れています。（{CIUtil.SDateToShowWDate(hokenInf.StartDate)} ～ {CIUtil.SDateToShowWDate(hokenInf.EndDate)}）"),
                        alertFg);
                }
            }

            return new WarningMemoModel();
        }

        private WarningMemoModel CheckHokenHasDateConfirmed(HokenInfModel hokenInf, int alertFg = 0)
        {
            if (hokenInf == null || hokenInf.IsReceKisaiOrNoHoken) return new WarningMemoModel();

            if (hokenInf.HasDateConfirmed == false)
            {
                if (hokenInf.IsHoken)
                {
                    return AddWarning(
                         string.Format(
                             $"【保険確認】 保険が未確認です。（最終確認日：{CIUtil.SDateToShowWDate(ptHokenInf.LastDateConfirmed)}）"),
                         alertFg);
                }
            }

            return new WarningMemoModel();
        }

        private WarningMemoModel AddWarning(string msg, int alertFg = 0)
        {
            if (alertFg >= 1)
            {
                return new WarningMemoModel(msg, 1);
            }
            return new WarningMemoModel(msg, 0);

        }
    }
}
