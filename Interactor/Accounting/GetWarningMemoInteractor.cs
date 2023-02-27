using Domain.Models.Accounting;
using Domain.Models.Insurance;
using Domain.Models.Reception;
using Helper.Common;
using UseCase.Accounting.WarningMemo;

namespace Interactor.Accounting
{
    public class GetWarningMemoInteractor : IGetWarningMemoInputPort
    {
        private readonly IAccountingRepository _accountingRepository;

        public GetWarningMemoInteractor(IAccountingRepository accountingRepository)
        {
            _accountingRepository = accountingRepository;
        }

        public GetWarningMemoOutputData Handle(GetWarningMemoInputData inputData)
        {
            try
            {
                //Get Raiin Inf
                var raiinInf = _accountingRepository.GetListRaiinInf(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.RaiinNo).ToList();

                if (!raiinInf.Any())
                    return new GetWarningMemoOutputData(new(), GetWarningMemoStatus.NoData);

                //Get Warning Memo
                var warning = GetWarningMemo(inputData.HpId, inputData.PtId, inputData.SinDate, raiinInf);

                return new GetWarningMemoOutputData(warning, GetWarningMemoStatus.Successed);

            }
            finally
            {
                _accountingRepository.ReleaseResource();
            }
        }

        private List<WarningMemoDto> GetWarningMemo(int hpId, long ptId, int sinDate, List<ReceptionDto> raiinInfModels)
        {
            var warningMemoDto = new List<WarningMemoDto>();
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

                if (raiin.HokenPatternModel.HokenInfModels != null &&
                    raiin.HokenPatternModel.HokenInfModels.HokenId > 0)
                {
                    var checkExpirated = CheckHokenIsExpirated(raiin.RaiinNo, raiin.HokenPatternModel.HokenInfModels);
                    if (!string.IsNullOrEmpty(checkExpirated.Memo))
                        warningMemoDto.Add(checkExpirated);

                    var checkHasDateConfirm = CheckHokenHasDateConfirmed(raiin.RaiinNo, raiin.HokenPatternModel.HokenInfModels);
                    if (!string.IsNullOrEmpty(checkHasDateConfirm.Memo))
                        warningMemoDto.Add(checkHasDateConfirm);
                }

                if (raiin.HokenPatternModel.Kohi1 != null &&
                    raiin.HokenPatternModel.Kohi1.HokenId > 0 &&
                    !CheckIdIsExits(listKohiId, raiin.HokenPatternModel.Kohi1.HokenId))
                {
                    listKohiId.Add(raiin.HokenPatternModel.Kohi1.HokenId);
                    var checkKohi = CheckKohiIsExpirated(raiin.RaiinNo, raiin.HokenPatternModel.Kohi1);
                    if (!string.IsNullOrEmpty(checkKohi.Memo))
                        warningMemoDto.Add(checkKohi);

                    var checkHasDateConfirm = CheckKohiHasDateConfirmed(raiin.RaiinNo, raiin.HokenPatternModel.Kohi1);
                    if (!string.IsNullOrEmpty(checkHasDateConfirm.Memo))
                        warningMemoDto.Add(checkHasDateConfirm);
                }

                if (raiin.HokenPatternModel.Kohi2 != null &&
                    raiin.HokenPatternModel.Kohi2.HokenId > 0 &&
                    !CheckIdIsExits(listKohiId, raiin.HokenPatternModel.Kohi2.HokenId))
                {
                    listKohiId.Add(raiin.HokenPatternModel.Kohi2.HokenId);
                    var checkKohi = CheckKohiIsExpirated(raiin.RaiinNo, raiin.HokenPatternModel.Kohi2);
                    if (!string.IsNullOrEmpty(checkKohi.Memo))
                        warningMemoDto.Add(checkKohi);

                    var checkHasDateConfirm = CheckKohiHasDateConfirmed(raiin.RaiinNo, raiin.HokenPatternModel.Kohi2);
                    if (!string.IsNullOrEmpty(checkHasDateConfirm.Memo))
                        warningMemoDto.Add(checkHasDateConfirm);
                }

                if (raiin.HokenPatternModel.Kohi3 != null &&
                    raiin.HokenPatternModel.Kohi3.HokenId > 0 &&
                    !CheckIdIsExits(listKohiId, raiin.HokenPatternModel.Kohi3.HokenId))
                {
                    listKohiId.Add(raiin.HokenPatternModel.Kohi3.HokenId);
                    var checkKohi = CheckKohiIsExpirated(raiin.RaiinNo, raiin.HokenPatternModel.Kohi3);
                    if (!string.IsNullOrEmpty(checkKohi.Memo))
                        warningMemoDto.Add(checkKohi);

                    var checkHasDateConfirm = CheckKohiHasDateConfirmed(raiin.RaiinNo, raiin.HokenPatternModel.Kohi3);
                    if (!string.IsNullOrEmpty(checkHasDateConfirm.Memo))
                        warningMemoDto.Add(checkHasDateConfirm);
                }

                if (raiin.HokenPatternModel.Kohi4 != null &&
                    raiin.HokenPatternModel.Kohi4.HokenId > 0 &&
                    !CheckIdIsExits(listKohiId, raiin.HokenPatternModel.Kohi4.HokenId))
                {
                    listKohiId.Add(raiin.HokenPatternModel.Kohi4.HokenId);
                    var checkKohi = CheckKohiIsExpirated(raiin.RaiinNo, raiin.HokenPatternModel.Kohi4);
                    if (!string.IsNullOrEmpty(checkKohi.Memo))
                        warningMemoDto.Add(checkKohi);

                    var checkHasDateConfirm = CheckKohiHasDateConfirmed(raiin.RaiinNo, raiin.HokenPatternModel.Kohi4);
                    if (!string.IsNullOrEmpty(checkHasDateConfirm.Memo))
                        warningMemoDto.Add(checkHasDateConfirm);
                }
            }

            var listCalcLog = _accountingRepository.GetCalcLog(hpId, ptId, sinDate, raiinInfModels.Select(x => x.RaiinNo).ToList());

            if (listCalcLog != null && listCalcLog.Count > 0)
            {
                foreach (var calcLog in listCalcLog)
                {
                    if (!string.IsNullOrEmpty(calcLog.Text))
                        warningMemoDto.Add(new WarningMemoDto(calcLog.RaiinNo, calcLog.Text ?? string.Empty, calcLog.LogSbt));
                }
            }
            return warningMemoDto;
        }
        private WarningMemoDto CheckHokenIsExpirated(long raiinNo, HokenInfModel ptHokenInf, int alertFg = 1)
        {
            if (ptHokenInf == null || ptHokenInf.IsReceKisaiOrNoHoken) return new();

            if (ptHokenInf.IsExpirated == true)
            {
                if (ptHokenInf.IsHoken)
                {
                    return new WarningMemoDto(raiinNo, string.Format(
                        $"【保険確認】 主保険の有効期限が切れています。（{CIUtil.SDateToShowWDate(ptHokenInf.StartDate)} ～ {CIUtil.SDateToShowWDate(ptHokenInf.EndDate)}）"),
                        alertFg);
                }
                else if (ptHokenInf.IsRousai)
                {
                    return new WarningMemoDto(raiinNo, string.Format(
                        $"【保険確認】 労災保険の有効期限が切れています。（{CIUtil.SDateToShowWDate(ptHokenInf.StartDate)} ～ {CIUtil.SDateToShowWDate(ptHokenInf.EndDate)}）"),
                        alertFg);
                }
                else if (ptHokenInf.IsJibai)
                {
                    return new WarningMemoDto(raiinNo, string.Format(
                        $"【保険確認】 自賠責保険の有効期限が切れています。（{CIUtil.SDateToShowWDate(ptHokenInf.StartDate)} ～ {CIUtil.SDateToShowWDate(ptHokenInf.EndDate)}）"),
                        alertFg);
                }
            }
            return new();
        }

        private WarningMemoDto CheckHokenHasDateConfirmed(long raiinNo, HokenInfModel ptHokenInf, int alertFg = 0)
        {
            if (ptHokenInf == null || ptHokenInf.IsReceKisaiOrNoHoken) return new();

            if (ptHokenInf.HasDateConfirmed == false)
            {
                if (ptHokenInf.IsHoken)
                {
                    return new WarningMemoDto(raiinNo,
                        string.Format(
                            $"【保険確認】 保険が未確認です。（最終確認日：{CIUtil.SDateToShowWDate(ptHokenInf.LastDateConfirmed)}）"),
                        alertFg);
                }
            }
            return new();
        }

        private WarningMemoDto CheckKohiIsExpirated(long raiinNo, KohiInfModel ptKohiModel, int alertFg = 1)
        {
            if (ptKohiModel == null) return new();
            if (ptKohiModel.IsExpirated == true)
            {
                if (!string.IsNullOrWhiteSpace(ptKohiModel.FutansyaNo))
                {
                    return new WarningMemoDto(raiinNo,
                        string.Format(
                            $"【保険確認】 公費（{ptKohiModel.FutansyaNo}）の有効期限が切れています。（{CIUtil.SDateToShowWDate(ptKohiModel.StartDate)} ～ {CIUtil.SDateToShowWDate(ptKohiModel.EndDate)}）"),
                        alertFg);
                }
                else
                {
                    return new WarningMemoDto(raiinNo,
                        string.Format(
                            $"【保険確認】 公費 の有効期限が切れています。（{CIUtil.SDateToShowWDate(ptKohiModel.StartDate)} ～ {CIUtil.SDateToShowWDate(ptKohiModel.EndDate)}）"),
                        alertFg);
                }
            }
            return new();
        }

        private WarningMemoDto CheckKohiHasDateConfirmed(long raiinNo, KohiInfModel ptKohiModel, int alertFg = 0)
        {
            if (ptKohiModel == null) return new();
            if (ptKohiModel.HasDateConfirmed == false)
            {
                if (!string.IsNullOrWhiteSpace(ptKohiModel.FutansyaNo))
                {
                    return new WarningMemoDto(raiinNo, string.Format(
                            $"【保険確認】 公費（{ptKohiModel.FutansyaNo}）の保険証が未確認です。（最終確認日：{CIUtil.SDateToShowWDate(ptKohiModel.LastDateConfirmed)}）"),
                        alertFg);
                }
                else
                {
                    return new WarningMemoDto(raiinNo,
                        string.Format(
                            $"【保険確認】 公費 の保険証が未確認です。（最終確認日：{CIUtil.SDateToShowWDate(ptKohiModel.LastDateConfirmed)}）"),
                        alertFg);
                }
            }
            return new();
        }
    }
}
