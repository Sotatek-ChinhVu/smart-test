using Domain.CalculationInf;
using Domain.Models.Medical;
using Domain.Models.Receipt;
using Domain.Models.Receipt.Recalculation;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;

namespace Interactor.Recalculation
{
    public class Recalculation : IRecalculation
    {
        private readonly ICalculationInfRepository _calculation;

        public Recalculation(ICalculationInfRepository calculation)
        {
            _calculation = calculation;
        }

        public void CheckErrorInMonth(int hpId, int seikyuYm, List<long> ptIds)
        {
            try
            {
                IsStopCalc = false;
                var AllCheckCount = _calculation.GetCountReceInfs(hpId, ptIds, seikyuYm);
                CheckedCount = 0;

                var receCheckOpts = _calculation.GetReceCheckOpts(hpId);
                var receInfModels = _calculation.GetReceInfModels(hpId, ptIds, seikyuYm);

                var newReceCheckErrs = new List<ReceCheckErr>();

                foreach (var receInfModel in receInfModels)
                {
                    if (IsStopCalc) break;
                    if (CancellationToken.IsCancellationRequested) return;
                    _calculation.ClearReceCmtErr(receInfModel.HpId, receInfModel.PtId, receInfModel.HokenId, receInfModel.SinYm);
                    var sinKouiCounts = _calculation.GetSinKouiCounts(receInfModel.HpId, receInfModel.PtId, receInfModel.SinYm, receInfModel.HokenId);

                    CheckHoken(newReceCheckErrs , receInfModel, receCheckOpts, sinKouiCounts);
                    CheckByomei(receInfModel);
                    CheckOrder(receInfModel);
                    CheckRosai(receInfModel);
                    CheckAftercare(receInfModel);

                    CheckedCount++;
                }
                _commandHandler.SaveChanged();
                PrintReceCheck(seikyuYm, ptIds);
            }
            finally
            {
                Log.WriteLogEnd(ModuleNameConst.EmrCommonView, this, nameof(CheckErrorInMonth), ICDebugConf.logLevel);
            }
        }

        private void CheckHoken(List<ReceCheckErr> receCheckErrs,ReceInfModel receInfModel, List<ReceCheckOptModel> receCheckOpts, List<SinKouiCountModel> sinKouiCounts)
        {
            //expired
            if (receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.ExpiredEndDateHokenErrCd))
            {
                try
                {
                    if (sinKouiCounts.Count > 0)
                    {
                        //hoken
                        if (receInfModel.HokenId > 0 && receInfModel.Houbetu.AsInteger() != 0)
                        {
                            //E1002 start date
                            var firstSinKouiCount = sinKouiCounts.OrderBy(p => p.SinDate).FirstOrDefault(p => p.PtHokenPatterns.Any(q => q.HokenId == receInfModel.HokenId));
                            if (firstSinKouiCount != null)
                            {
                                if (receInfModel.PtHokenInf.StartDate > 0 && receInfModel.PtHokenInf.StartDate > firstSinKouiCount.SinDate)
                                {
                                    _commandHandler.InsertReceCmtErr(receInfModel, ReceErrCdConst.ExpiredStartDateHokenErrCd,
                                        ReceErrCdConst.ExpiredStartDateHokenErrMsg, "（" + CIUtil.SDateToShowSWDate(receInfModel.PtHokenInf.StartDate) + "～）", HOKEN_CHAR);
                                }
                            }

                            //E1001 end date
                            var lastSinKouiCount = _sinKouiCounts.OrderBy(p => p.SinDate).LastOrDefault(p => p.PtHokenPatterns.Any(q => q.HokenId == receInfModel.HokenId));
                            if (lastSinKouiCount != null)
                            {
                                if (receInfModel.HokenEndDate > 0 && receInfModel.HokenEndDate < lastSinKouiCount.SinDate)
                                {
                                    _commandHandler.InsertReceCmtErr(receInfModel, ReceErrCdConst.ExpiredEndDateHokenErrCd,
                                        ReceErrCdConst.ExpiredEndDateHokenErrMsg, "（～" + CIUtil.SDateToShowSWDate(receInfModel.HokenEndDate) + "）", HOKEN_CHAR);
                                }
                            }
                        }
                        //kohi1
                        if (receInfModel.Kohi1Id > 0 && receInfModel.Kohi1Houbetu.AsInteger() != 102)
                        {
                            var firstSinKouiCount = _sinKouiCounts.OrderBy(p => p.SinDate).FirstOrDefault(p => p.PtHokenPatterns
                                .Any(q => q.Kohi1Id == receInfModel.Kohi1Id || q.Kohi2Id == receInfModel.Kohi1Id || q.Kohi3Id == receInfModel.Kohi1Id || q.Kohi4Id == receInfModel.Kohi1Id));
                            if (firstSinKouiCount != null)
                            {
                                if (receInfModel.Kohi1StartDate > 0 && receInfModel.Kohi1StartDate > firstSinKouiCount.SinDate)
                                {
                                    _commandHandler.InsertReceCmtErr(receInfModel, ReceErrCdConst.ExpiredStartDateHokenErrCd,
                                        ReceErrCdConst.ExpiredStartDateHokenErrMsg, "（" + CIUtil.SDateToShowSWDate(receInfModel.Kohi1StartDate) + "～）", KOHI1_CHAR);
                                }
                            }

                            var lastSinKouiCount = _sinKouiCounts.OrderBy(p => p.SinDate).LastOrDefault(p => p.PtHokenPatterns
                                .Any(q => q.Kohi1Id == receInfModel.Kohi1Id || q.Kohi2Id == receInfModel.Kohi1Id || q.Kohi3Id == receInfModel.Kohi1Id || q.Kohi4Id == receInfModel.Kohi1Id));
                            if (lastSinKouiCount != null)
                            {
                                if (receInfModel.Kohi1EndDate > 0 && receInfModel.Kohi1EndDate < lastSinKouiCount.SinDate)
                                {
                                    _commandHandler.InsertReceCmtErr(receInfModel, ReceErrCdConst.ExpiredEndDateHokenErrCd,
                                        ReceErrCdConst.ExpiredEndDateHokenErrMsg, "（～" + CIUtil.SDateToShowSWDate(receInfModel.Kohi1EndDate) + "）", KOHI1_CHAR);
                                }
                            }
                        }
                        //kohi2
                        if (receInfModel.Kohi2Id > 0 && receInfModel.Kohi2Houbetu.AsInteger() != 102)
                        {
                            var firstSinKouiCount = _sinKouiCounts.OrderBy(p => p.SinDate).FirstOrDefault(p => p.PtHokenPatterns
                                .Any(q => q.Kohi1Id == receInfModel.Kohi2Id || q.Kohi2Id == receInfModel.Kohi2Id || q.Kohi3Id == receInfModel.Kohi2Id || q.Kohi4Id == receInfModel.Kohi2Id));
                            if (firstSinKouiCount != null)
                            {
                                if (receInfModel.Kohi2StartDate > 0 && receInfModel.Kohi2StartDate > firstSinKouiCount.SinDate)
                                {
                                    _commandHandler.InsertReceCmtErr(receInfModel, ReceErrCdConst.ExpiredStartDateHokenErrCd,
                                        ReceErrCdConst.ExpiredStartDateHokenErrMsg, "（" + CIUtil.SDateToShowSWDate(receInfModel.Kohi2StartDate) + "～）", KOHI2_CHAR);
                                }
                            }

                            var lastSinKouiCount = _sinKouiCounts.OrderBy(p => p.SinDate).LastOrDefault(p => p.PtHokenPatterns
                                .Any(q => q.Kohi1Id == receInfModel.Kohi2Id || q.Kohi2Id == receInfModel.Kohi2Id || q.Kohi3Id == receInfModel.Kohi2Id || q.Kohi4Id == receInfModel.Kohi2Id));
                            if (lastSinKouiCount != null)
                            {
                                if (receInfModel.Kohi2EndDate > 0 && receInfModel.Kohi2EndDate < lastSinKouiCount.SinDate)
                                {
                                    _commandHandler.InsertReceCmtErr(receInfModel, ReceErrCdConst.ExpiredEndDateHokenErrCd,
                                        ReceErrCdConst.ExpiredEndDateHokenErrMsg, "（～" + CIUtil.SDateToShowSWDate(receInfModel.Kohi2EndDate) + "）", KOHI2_CHAR);
                                }
                            }
                        }
                        //kohi3
                        if (receInfModel.Kohi3Id > 0 && receInfModel.Kohi3Houbetu.AsInteger() != 102)
                        {
                            var firstSinKouiCount = _sinKouiCounts.OrderBy(p => p.SinDate).FirstOrDefault(p => p.PtHokenPatterns
                                .Any(q => q.Kohi1Id == receInfModel.Kohi3Id || q.Kohi2Id == receInfModel.Kohi3Id || q.Kohi3Id == receInfModel.Kohi3Id || q.Kohi4Id == receInfModel.Kohi3Id));
                            if (firstSinKouiCount != null)
                            {
                                if (receInfModel.Kohi3StartDate > 0 && receInfModel.Kohi3StartDate > firstSinKouiCount.SinDate)
                                {
                                    _commandHandler.InsertReceCmtErr(receInfModel, ReceErrCdConst.ExpiredStartDateHokenErrCd,
                                        ReceErrCdConst.ExpiredStartDateHokenErrMsg, "（" + CIUtil.SDateToShowSWDate(receInfModel.Kohi3StartDate) + "～）", KOHI3_CHAR);
                                }
                            }

                            var lastSinKouiCount = _sinKouiCounts.OrderBy(p => p.SinDate).LastOrDefault(p => p.PtHokenPatterns
                                .Any(q => q.Kohi1Id == receInfModel.Kohi3Id || q.Kohi2Id == receInfModel.Kohi3Id || q.Kohi3Id == receInfModel.Kohi3Id || q.Kohi4Id == receInfModel.Kohi3Id));
                            if (lastSinKouiCount != null)
                            {
                                if (receInfModel.Kohi3EndDate > 0 && receInfModel.Kohi3EndDate < lastSinKouiCount.SinDate)
                                {
                                    _commandHandler.InsertReceCmtErr(receInfModel, ReceErrCdConst.ExpiredEndDateHokenErrCd,
                                        ReceErrCdConst.ExpiredEndDateHokenErrMsg, "（～" + CIUtil.SDateToShowSWDate(receInfModel.Kohi3EndDate) + "）", KOHI3_CHAR);
                                }
                            }
                        }
                        //kohi4
                        if (receInfModel.Kohi4Id > 0 && receInfModel.Kohi4Houbetu.AsInteger() != 102)
                        {
                            var firstSinKouiCount = _sinKouiCounts.OrderBy(p => p.SinDate).FirstOrDefault(p => p.PtHokenPatterns
                                .Any(q => q.Kohi1Id == receInfModel.Kohi4Id || q.Kohi2Id == receInfModel.Kohi4Id || q.Kohi3Id == receInfModel.Kohi4Id || q.Kohi4Id == receInfModel.Kohi4Id));
                            if (firstSinKouiCount != null)
                            {
                                if (receInfModel.Kohi4StartDate > 0 && receInfModel.Kohi4StartDate > firstSinKouiCount.SinDate)
                                {
                                    _commandHandler.InsertReceCmtErr(receInfModel, ReceErrCdConst.ExpiredStartDateHokenErrCd,
                                        ReceErrCdConst.ExpiredStartDateHokenErrMsg, "（" + CIUtil.SDateToShowSWDate(receInfModel.Kohi4StartDate) + "～）", KOHI4_CHAR);
                                }
                            }

                            var lastSinKouiCount = _sinKouiCounts.OrderBy(p => p.SinDate).LastOrDefault(p => p.PtHokenPatterns
                                .Any(q => q.Kohi1Id == receInfModel.Kohi4Id || q.Kohi2Id == receInfModel.Kohi4Id || q.Kohi3Id == receInfModel.Kohi4Id || q.Kohi4Id == receInfModel.Kohi4Id));
                            if (lastSinKouiCount != null)
                            {
                                if (receInfModel.Kohi4EndDate > 0 && receInfModel.Kohi4EndDate < lastSinKouiCount.SinDate)
                                {
                                    _commandHandler.InsertReceCmtErr(receInfModel, ReceErrCdConst.ExpiredEndDateHokenErrCd,
                                        ReceErrCdConst.ExpiredEndDateHokenErrMsg, "（～" + CIUtil.SDateToShowSWDate(receInfModel.Kohi4EndDate) + "）", KOHI4_CHAR);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteLogError(_modulName, this, nameof(CheckHoken), ex, $"CheckErrCd: {ReceErrCdConst.ExpiredEndDateHokenErrCd}, PtId: {receInfModel.PtId}");
                }
            }

            //E1003 unconfirmed
            if (_receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.UnConfirmedHokenErrCd))
            {
                try
                {
                    if (receInfModel.HokenId > 0 && !receInfModel.IsHokenConfirmed)
                    {
                        string latestConfirmedDate = string.Empty;
                        if (receInfModel.HokenChecks?.Count > 0)
                        {
                            latestConfirmedDate = CIUtil.SDateToShowSWDate(CIUtil.DateTimeToInt(receInfModel.HokenChecks.OrderByDescending(p => p.CheckDate).FirstOrDefault().CheckDate));
                        }
                        _commandHandler.InsertReceCmtErr(receInfModel, ReceErrCdConst.UnConfirmedHokenErrCd,
                                ReceErrCdConst.UnConfirmedHokenErrMsg, "（最終確認: " + latestConfirmedDate + "）", HOKEN_CHAR);
                    }
                    if (receInfModel.Kohi1Id > 0 && !receInfModel.IsKohi1Confirmed)
                    {
                        string latestConfirmedDate = string.Empty;
                        if (receInfModel.Kohi1Checks?.Count > 0)
                        {
                            latestConfirmedDate = CIUtil.SDateToShowSWDate(CIUtil.DateTimeToInt(receInfModel.Kohi1Checks.OrderByDescending(p => p.CheckDate).FirstOrDefault().CheckDate));
                        }
                        _commandHandler.InsertReceCmtErr(receInfModel, ReceErrCdConst.UnConfirmedHokenErrCd,
                                ReceErrCdConst.UnConfirmedHokenErrMsg, "（最終確認: " + latestConfirmedDate + "）", KOHI1_CHAR);
                    }
                    if (receInfModel.Kohi2Id > 0 && !receInfModel.IsKohi2Confirmed)
                    {
                        string latestConfirmedDate = string.Empty;
                        if (receInfModel.Kohi2Checks?.Count > 0)
                        {
                            latestConfirmedDate = CIUtil.SDateToShowSWDate(CIUtil.DateTimeToInt(receInfModel.Kohi2Checks.OrderByDescending(p => p.CheckDate).FirstOrDefault().CheckDate));
                        }
                        _commandHandler.InsertReceCmtErr(receInfModel, ReceErrCdConst.UnConfirmedHokenErrCd,
                                ReceErrCdConst.UnConfirmedHokenErrMsg, "（最終確認: " + latestConfirmedDate + "）", KOHI2_CHAR);
                    }
                    if (receInfModel.Kohi3Id > 0 && !receInfModel.IsKohi3Confirmed)
                    {
                        string latestConfirmedDate = string.Empty;
                        if (receInfModel.Kohi3Checks?.Count > 0)
                        {
                            latestConfirmedDate = CIUtil.SDateToShowSWDate(CIUtil.DateTimeToInt(receInfModel.Kohi3Checks.OrderByDescending(p => p.CheckDate).FirstOrDefault().CheckDate));
                        }
                        _commandHandler.InsertReceCmtErr(receInfModel, ReceErrCdConst.UnConfirmedHokenErrCd,
                                ReceErrCdConst.UnConfirmedHokenErrMsg, "（最終確認: " + latestConfirmedDate + "）", KOHI3_CHAR);
                    }
                    if (receInfModel.Kohi4Id > 0 && !receInfModel.IsKohi4Confirmed)
                    {
                        string latestConfirmedDate = string.Empty;
                        if (receInfModel.Kohi4Checks?.Count > 0)
                        {
                            latestConfirmedDate = CIUtil.SDateToShowSWDate(CIUtil.DateTimeToInt(receInfModel.Kohi4Checks.OrderByDescending(p => p.CheckDate).FirstOrDefault().CheckDate));
                        }
                        _commandHandler.InsertReceCmtErr(receInfModel, ReceErrCdConst.UnConfirmedHokenErrCd,
                                ReceErrCdConst.UnConfirmedHokenErrMsg, "（最終確認: " + latestConfirmedDate + "）", KOHI4_CHAR);
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteLogError(_modulName, this, nameof(CheckHoken), ex, $"CheckErrCd: {ReceErrCdConst.UnConfirmedHokenErrCd}, PtId: {receInfModel.PtId}");
                }
            }
            Log.WriteLogEnd(ModuleNameConst.EmrCommonView, this, nameof(CheckHoken), ICDebugConf.logLevel);
        }
    }
}
