using Domain.CalculationInf;
using Domain.Models.CalculationInf;
using Domain.Models.Medical;
using Domain.Models.Receipt;
using Domain.Models.Receipt.Recalculation;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class CalculationInfRepository : RepositoryBase, ICalculationInfRepository
    {
        private const string HOKEN_CHAR = "0";
        private const string KOHI1_CHAR = "1";
        private const string KOHI2_CHAR = "2";
        private const string KOHI3_CHAR = "3";
        private const string KOHI4_CHAR = "4";
        private const string FREE_WORD = "0000999";
        private const string SUSPECTED_SUFFIX = "の疑い";
        private const string LEFT = "左";
        private const string RIGHT = "右";
        private const string BOTH = "両";
        private const string LEFT_RIGHT = "左右";
        private const string RIGHT_LEFT = "右左";

        public CalculationInfRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public IEnumerable<CalculationInfModel> GetListDataCalculationInf(int hpId, long ptId)
        {
            var dataCalculation = NoTrackingDataContext.PtSanteiConfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == 0)
                .Select(x => new CalculationInfModel(
                        x.HpId,
                        x.PtId,
                        x.KbnNo,
                        x.EdaNo,
                        x.KbnVal,
                        x.StartDate,
                        x.EndDate,
                        x.SeqNo
                    ))
                .ToList();

            return dataCalculation;

        }

        public void CheckErrorInMonth(int hpId, int userId, string userName, int seikyuYm, List<long> ptIds)
        {
            try
            {
                IsStopCalc = false;
                var AllCheckCount = GetCountReceInfs(hpId, ptIds, seikyuYm);
                var CheckedCount = 0;

                var receCheckOpts = GetReceCheckOpts(hpId);
                var receInfModels = GetReceInfModels(hpId, ptIds, seikyuYm);

                var newReceCheckErrs = new List<ReceCheckErr>();

                foreach (var receInfModel in receInfModels)
                {
                    if (IsStopCalc) break;
                    if (CancellationToken.IsCancellationRequested) return;
                    var oldReceCheckErrs = ClearReceCmtErr(hpId, receInfModel.PtId, receInfModel.HokenId, receInfModel.SinYm);
                    var sinKouiCounts = GetSinKouiCounts(hpId, receInfModel.PtId, receInfModel.SinYm, receInfModel.HokenId);

                    CheckHoken(hpId, userId, userName, receInfModel, receCheckOpts, sinKouiCounts, oldReceCheckErrs, newReceCheckErrs);
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

        private void CheckHoken(int hpId, int userId, string userName, ReceInfModel receInfModel, List<ReceCheckOptModel> receCheckOpts, List<SinKouiCountModel> sinKouiCounts, List<ReceCheckErr> oldReceCheckErrs, List<ReceCheckErr> newReceCheckErrs)
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
                                    InsertReceCmtErr(hpId, userId, userName, oldReceCheckErrs, newReceCheckErrs, receInfModel, ReceErrCdConst.ExpiredStartDateHokenErrCd,
                                        ReceErrCdConst.ExpiredStartDateHokenErrMsg, "（" + CIUtil.SDateToShowSWDate(receInfModel.PtHokenInf.StartDate) + "～）", HOKEN_CHAR);
                                }
                            }

                            //E1001 end date
                            var lastSinKouiCount = sinKouiCounts.OrderBy(p => p.SinDate).LastOrDefault(p => p.PtHokenPatterns.Any(q => q.HokenId == receInfModel.HokenId));
                            if (lastSinKouiCount != null)
                            {
                                if (receInfModel.PtHokenInf.EndDate > 0 && receInfModel.PtHokenInf.EndDate < lastSinKouiCount.SinDate)
                                {
                                    InsertReceCmtErr(hpId, userId, userName, oldReceCheckErrs, newReceCheckErrs, receInfModel, ReceErrCdConst.ExpiredEndDateHokenErrCd,
                                        ReceErrCdConst.ExpiredEndDateHokenErrMsg, "（～" + CIUtil.SDateToShowSWDate(receInfModel.PtHokenInf.EndDate) + "）", HOKEN_CHAR);
                                }
                            }
                        }
                        //kohi1
                        if (receInfModel.Kohi1Id > 0 && receInfModel.Kohi1Houbetu.AsInteger() != 102)
                        {
                            var firstSinKouiCount = sinKouiCounts.OrderBy(p => p.SinDate).FirstOrDefault(p => p.PtHokenPatterns
                                .Any(q => q.Kohi1Id == receInfModel.Kohi1Id || q.Kohi2Id == receInfModel.Kohi1Id || q.Kohi3Id == receInfModel.Kohi1Id || q.Kohi4Id == receInfModel.Kohi1Id));
                            if (firstSinKouiCount != null)
                            {
                                if (receInfModel.PtKohi1.StartDate > 0 && receInfModel.PtKohi1.StartDate > firstSinKouiCount.SinDate)
                                {
                                    InsertReceCmtErr(hpId, userId, userName, oldReceCheckErrs, newReceCheckErrs, receInfModel, ReceErrCdConst.ExpiredStartDateHokenErrCd,
                                        ReceErrCdConst.ExpiredStartDateHokenErrMsg, "（" + CIUtil.SDateToShowSWDate(receInfModel.PtKohi1.StartDate) + "～）", KOHI1_CHAR);
                                }
                            }

                            var lastSinKouiCount = sinKouiCounts.OrderBy(p => p.SinDate).LastOrDefault(p => p.PtHokenPatterns
                                .Any(q => q.Kohi1Id == receInfModel.Kohi1Id || q.Kohi2Id == receInfModel.Kohi1Id || q.Kohi3Id == receInfModel.Kohi1Id || q.Kohi4Id == receInfModel.Kohi1Id));
                            if (lastSinKouiCount != null)
                            {
                                if (receInfModel.PtKohi1.EndDate > 0 && receInfModel.PtKohi1.EndDate < lastSinKouiCount.SinDate)
                                {
                                    InsertReceCmtErr(receInfModel, ReceErrCdConst.ExpiredEndDateHokenErrCd,
                                        ReceErrCdConst.ExpiredEndDateHokenErrMsg, "（～" + CIUtil.SDateToShowSWDate(receInfModel.PtKohi1.EndDate) + "）", KOHI1_CHAR);
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

        private int GetCountReceInfs(int hpId, List<long> ptIds, int sinYm)
        {
            return NoTrackingDataContext.ReceInfs.Where(p => p.HpId == hpId && p.SeikyuYm == sinYm && (ptIds.Count == 0 || ptIds.Contains(p.PtId))).Count();
        }

        private List<ReceCheckOptModel> GetReceCheckOpts(int hpId)
        {
            var receCheckOpts = NoTrackingDataContext.ReceCheckOpts.Where(p => p.HpId == hpId)
                    .Select(x => new ReceCheckOptModel(x.ErrCd, x.CheckOpt, x.Biko ?? string.Empty, x.IsInvalid)).ToList();

            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.ExpiredEndDateHokenErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.ExpiredEndDateHokenErrCd));
            }
            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.UnConfirmedHokenErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.UnConfirmedHokenErrCd));
            }
            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.NotExistByomeiErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.NotExistByomeiErrCd));
            }
            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.CheckFirstVisit2003ByomeiErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.CheckFirstVisit2003ByomeiErrCd));
            }
            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.HasNotMainByomeiErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.HasNotMainByomeiErrCd));
            }
            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.InvalidByomeiErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.InvalidByomeiErrCd));
            }
            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.FreeTextLengthByomeiErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.FreeTextLengthByomeiErrCd));
            }
            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.CheckSuspectedByomeiErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.CheckSuspectedByomeiErrCd));
            }
            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.HasNotByomeiWithOdrErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.HasNotByomeiWithOdrErrCd));
            }
            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.ExpiredEndDateOdrErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.ExpiredEndDateOdrErrCd));
            }
            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.FirstExamFeeCheckErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.FirstExamFeeCheckErrCd));
            }
            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.SanteiCountCheckErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.SanteiCountCheckErrCd));
            }
            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.TokuzaiItemCheckErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.TokuzaiItemCheckErrCd));
            }
            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.ItemAgeCheckErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.ItemAgeCheckErrCd));
            }
            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.CommentCheckErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.CommentCheckErrCd));
            }
            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.ExceededDosageOdrErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.ExceededDosageOdrErrCd));
            }
            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.DuplicateOdrErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.DuplicateOdrErrCd));
            }

            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.ByomeiBuiOrderByomeiChekkuErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.ByomeiBuiOrderByomeiChekkuErrCd));
            }
            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.BuiOrderByomeiErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.BuiOrderByomeiErrCd));
            }
            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.DuplicateByomeiCheckErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.DuplicateByomeiCheckErrCd));
            }
            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.DuplicateSyusyokuByomeiCheckErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.DuplicateSyusyokuByomeiCheckErrCd));
            }
            if (!receCheckOpts.Any(p => p.ErrCd == ReceErrCdConst.AdditionItemErrCd))
            {
                receCheckOpts.Add(GetDefaultReceCheckOpt(ReceErrCdConst.AdditionItemErrCd));
            }
            return receCheckOpts;
        }

        private ReceCheckOptModel GetDefaultReceCheckOpt(string errCd)
        {
            var defaultReceCheckOpt = new ReceCheckOpt() { ErrCd = errCd };
            if (errCd == ReceErrCdConst.CheckSuspectedByomeiErrCd)
            {
                defaultReceCheckOpt.CheckOpt = 3;
            }
            return new ReceCheckOptModel(defaultReceCheckOpt.ErrCd, defaultReceCheckOpt.CheckOpt, defaultReceCheckOpt.Biko ?? string.Empty, defaultReceCheckOpt.IsInvalid);
        }

        private List<ReceInfModel> GetReceInfModels(int hpId, List<long> ptIds, int sinYM)
        {
            List<ReceInfModel> receInfModels = new List<ReceInfModel>();
            var ptInfs = NoTrackingDataContext.PtInfs.Where(p => p.HpId == hpId && p.IsDelete == DeleteTypes.None);

            var receStates = NoTrackingDataContext.ReceStatuses.Where(p => p.HpId == hpId && p.SeikyuYm == sinYM && (ptIds.Count > 0 ? ptIds.Contains(p.PtId) : true));

            var receInfs = NoTrackingDataContext.ReceInfs.Where(p => p.HpId == hpId && p.SeikyuYm == sinYM && (ptIds.Count > 0 ? ptIds.Contains(p.PtId) : true));

            var ptHokenInfs = NoTrackingDataContext.PtHokenInfs.Where(p => p.HpId == hpId && p.IsDeleted == DeleteTypes.None && (ptIds.Count > 0 ? ptIds.Contains(p.PtId) : true));

            var ptKohiInfs = NoTrackingDataContext.PtKohis.Where(p => p.HpId == hpId && p.IsDeleted == DeleteTypes.None && (ptIds.Count > 0 ? ptIds.Contains(p.PtId) : true));

            var receInfJoinPtInfQuery = from receInf in receInfs
                                        join ptInf in ptInfs
                                        on receInf.PtId equals ptInf.PtId
                                        select new
                                        {
                                            PtInf = ptInf,
                                            ReceInf = receInf
                                        };

            var query = from receInfJoinPtInf in receInfJoinPtInfQuery
                        join ptHokenInf in ptHokenInfs
                        on new { receInfJoinPtInf.ReceInf.PtId, receInfJoinPtInf.ReceInf.HokenId } equals new { ptHokenInf.PtId, ptHokenInf.HokenId } into tempPtHokenInfs
                        from tempPtHokenInf in tempPtHokenInfs.DefaultIfEmpty()
                        where receInfJoinPtInf.ReceInf.HokenId != 0
                        select new
                        {
                            PtInf = receInfJoinPtInf.PtInf,
                            ReceInf = receInfJoinPtInf.ReceInf,
                            PtHokenInf = tempPtHokenInf
                        };

            var query1 = from queryEntity in query
                         join ptKohiInf in ptKohiInfs
                         on new { queryEntity.ReceInf.PtId, HokenId = queryEntity.ReceInf.Kohi1Id } equals new { ptKohiInf.PtId, ptKohiInf.HokenId } into tempPtKohiInfs
                         from tempPtKohiInf in tempPtKohiInfs.DefaultIfEmpty()
                         select new
                         {
                             PtInf = queryEntity.PtInf,
                             ReceInf = queryEntity.ReceInf,
                             PtHokenInf = queryEntity.PtHokenInf,
                             PtKohi1Inf = tempPtKohiInf
                         };

            var query2 = from query1Entity in query1
                         join ptKohiInf in ptKohiInfs
                         on new { query1Entity.ReceInf.PtId, HokenId = query1Entity.ReceInf.Kohi2Id } equals new { ptKohiInf.PtId, ptKohiInf.HokenId } into tempPtKohiInfs
                         from tempPtKohiInf in tempPtKohiInfs.DefaultIfEmpty()
                         select new
                         {
                             PtInf = query1Entity.PtInf,
                             ReceInf = query1Entity.ReceInf,
                             PtHokenInf = query1Entity.PtHokenInf,
                             PtKohi1Inf = query1Entity.PtKohi1Inf,
                             PtKohi2Inf = tempPtKohiInf
                         };
            var query3 = from query2Entity in query2
                         join ptKohiInf in ptKohiInfs
                         on new { query2Entity.ReceInf.PtId, HokenId = query2Entity.ReceInf.Kohi3Id } equals new { ptKohiInf.PtId, ptKohiInf.HokenId } into tempPtKohiInfs
                         from tempPtKohiInf in tempPtKohiInfs.DefaultIfEmpty()
                         select new
                         {
                             PtInf = query2Entity.PtInf,
                             ReceInf = query2Entity.ReceInf,
                             PtHokenInf = query2Entity.PtHokenInf,
                             PtKohi1Inf = query2Entity.PtKohi1Inf,
                             PtKohi2Inf = query2Entity.PtKohi2Inf,
                             PtKohi3Inf = tempPtKohiInf
                         };
            var query4 = from query3Entity in query3
                         join ptKohiInf in ptKohiInfs
                         on new { query3Entity.ReceInf.PtId, HokenId = query3Entity.ReceInf.Kohi4Id } equals new { ptKohiInf.PtId, ptKohiInf.HokenId } into tempPtKohiInfs
                         from tempPtKohiInf in tempPtKohiInfs.DefaultIfEmpty()
                         select new
                         {
                             PtInf = query3Entity.PtInf,
                             ReceInf = query3Entity.ReceInf,
                             PtHokenInf = query3Entity.PtHokenInf,
                             PtKohi1Inf = query3Entity.PtKohi1Inf,
                             PtKohi2Inf = query3Entity.PtKohi2Inf,
                             PtKohi3Inf = query3Entity.PtKohi3Inf,
                             PtKohi4Inf = tempPtKohiInf
                         };

            var hokenChecks = NoTrackingDataContext.PtHokenChecks.Where(p => p.HpId == hpId &&
                                                                                             p.IsDeleted == DeleteTypes.None &&
                                                                                             (ptIds.Count > 0 ? ptIds.Contains(p.PtID) : true));

            var query5 = from query4entity in query4
                         join hokenCheck in hokenChecks.Where(p => p.HokenGrp == 1)
                         on new { query4entity.ReceInf.PtId, query4entity.ReceInf.HokenId } equals new { PtId = hokenCheck.PtID, hokenCheck.HokenId } into tempHokenChecks
                         select new
                         {
                             PtInf = query4entity.PtInf,
                             ReceInf = query4entity.ReceInf,
                             PtHokenInf = query4entity.PtHokenInf,
                             PtKohi1Inf = query4entity.PtKohi1Inf,
                             PtKohi2Inf = query4entity.PtKohi2Inf,
                             PtKohi3Inf = query4entity.PtKohi3Inf,
                             PtKohi4Inf = query4entity.PtKohi4Inf,
                             HokenChecks = tempHokenChecks

                         };

            var query6 = from query5entity in query5
                         join hokenCheck in hokenChecks.Where(p => p.HokenGrp == 2)
                         on new { query5entity.ReceInf.PtId, HokenId = query5entity.ReceInf.Kohi1Id } equals new { PtId = hokenCheck.PtID, hokenCheck.HokenId } into tempHokenChecks
                         select new
                         {
                             PtInf = query5entity.PtInf,
                             ReceInf = query5entity.ReceInf,
                             PtHokenInf = query5entity.PtHokenInf,
                             PtKohi1Inf = query5entity.PtKohi1Inf,
                             PtKohi2Inf = query5entity.PtKohi2Inf,
                             PtKohi3Inf = query5entity.PtKohi3Inf,
                             PtKohi4Inf = query5entity.PtKohi4Inf,
                             HokenChecks = query5entity.HokenChecks,
                             Kohi1Checks = tempHokenChecks
                         };

            var query7 = from query6entity in query6
                         join hokenCheck in hokenChecks.Where(p => p.HokenGrp == 2)
                         on new { query6entity.ReceInf.PtId, HokenId = query6entity.ReceInf.Kohi2Id } equals new { PtId = hokenCheck.PtID, hokenCheck.HokenId } into tempHokenChecks
                         select new
                         {
                             PtInf = query6entity.PtInf,
                             ReceInf = query6entity.ReceInf,
                             PtHokenInf = query6entity.PtHokenInf,
                             PtKohi1Inf = query6entity.PtKohi1Inf,
                             PtKohi2Inf = query6entity.PtKohi2Inf,
                             PtKohi3Inf = query6entity.PtKohi3Inf,
                             PtKohi4Inf = query6entity.PtKohi4Inf,
                             HokenChecks = query6entity.HokenChecks,
                             Kohi1Checks = query6entity.Kohi1Checks,
                             Kohi2Checks = tempHokenChecks
                         };

            var query8 = from query7entity in query7
                         join hokenCheck in hokenChecks.Where(p => p.HokenGrp == 2)
                         on new { query7entity.ReceInf.PtId, HokenId = query7entity.ReceInf.Kohi3Id } equals new { PtId = hokenCheck.PtID, hokenCheck.HokenId } into tempHokenChecks
                         select new
                         {
                             PtInf = query7entity.PtInf,
                             ReceInf = query7entity.ReceInf,
                             PtHokenInf = query7entity.PtHokenInf,
                             PtKohi1Inf = query7entity.PtKohi1Inf,
                             PtKohi2Inf = query7entity.PtKohi2Inf,
                             PtKohi3Inf = query7entity.PtKohi3Inf,
                             PtKohi4Inf = query7entity.PtKohi4Inf,
                             HokenChecks = query7entity.HokenChecks,
                             Kohi1Checks = query7entity.Kohi1Checks,
                             Kohi2Checks = query7entity.Kohi2Checks,
                             Kohi3Checks = tempHokenChecks
                         };

            var query9 = from query8entity in query8
                         join hokenCheck in hokenChecks.Where(p => p.HokenGrp == 2)
                         on new { query8entity.ReceInf.PtId, HokenId = query8entity.ReceInf.Kohi4Id } equals new { PtId = hokenCheck.PtID, hokenCheck.HokenId } into tempHokenChecks
                         select new
                         {
                             PtInf = query8entity.PtInf,
                             ReceInf = query8entity.ReceInf,
                             PtHokenInf = query8entity.PtHokenInf,
                             PtKohi1Inf = query8entity.PtKohi1Inf,
                             PtKohi2Inf = query8entity.PtKohi2Inf,
                             PtKohi3Inf = query8entity.PtKohi3Inf,
                             PtKohi4Inf = query8entity.PtKohi4Inf,
                             HokenChecks = query8entity.HokenChecks,
                             Kohi1Checks = query8entity.Kohi1Checks,
                             Kohi2Checks = query8entity.Kohi2Checks,
                             Kohi3Checks = query8entity.Kohi3Checks,
                             Kohi4Checks = tempHokenChecks
                         };

            var query10 = from query9entity in query9
                          join receState in receStates
                          on new { query9entity.ReceInf.PtId, query9entity.ReceInf.HokenId } equals new { receState.PtId, receState.HokenId } into tempReceStates
                          select new
                          {
                              PtInf = query9entity.PtInf,
                              ReceInf = query9entity.ReceInf,
                              PtHokenInf = query9entity.PtHokenInf,
                              PtKohi1Inf = query9entity.PtKohi1Inf,
                              PtKohi2Inf = query9entity.PtKohi2Inf,
                              PtKohi3Inf = query9entity.PtKohi3Inf,
                              PtKohi4Inf = query9entity.PtKohi4Inf,
                              HokenChecks = query9entity.HokenChecks,
                              Kohi1Checks = query9entity.Kohi1Checks,
                              Kohi2Checks = query9entity.Kohi2Checks,
                              Kohi3Checks = query9entity.Kohi3Checks,
                              Kohi4Checks = query9entity.Kohi4Checks,
                              ReceStatus = tempReceStates.FirstOrDefault()
                          };

            foreach (var entity in query10)
            {
                receInfModels.Add(new ReceInfModel(
                    ConvertToReceInfModel(entity.ReceInf)
                    ));
            }
            return receInfModels;
        }

        private ReceInfModel ConvertToReceInfModel(ReceInf receInf)
        {
            return new ReceInfModel(
                       receInf.HpId,
                       receInf.SeikyuYm,
                       receInf.PtId,
                       receInf.SinYm,
                       receInf.HokenId,
                       receInf.HokenId2,
                       receInf.KaId,
                       receInf.TantoId,
                       receInf.ReceSbt ?? string.Empty,
                       receInf.HokenKbn,
                       receInf.HokenSbtCd,
                       receInf.Houbetu ?? string.Empty,
                       receInf.Kohi1Id,
                       receInf.Kohi2Id,
                       receInf.Kohi3Id,
                       receInf.Kohi4Id,
                       receInf.Kohi1Houbetu ?? string.Empty,
                       receInf.Kohi2Houbetu ?? string.Empty,
                       receInf.Kohi3Houbetu ?? string.Empty,
                       receInf.Kohi4Houbetu ?? string.Empty,
                       receInf.HonkeKbn,
                       receInf.Tokki1 ?? string.Empty,
                       receInf.Tokki2 ?? string.Empty,
                       receInf.Tokki3 ?? string.Empty,
                       receInf.Tokki4 ?? string.Empty,
                       receInf.Tokki5 ?? string.Empty,
                       receInf?.HokenNissu ?? -1,
                       receInf?.Kohi1Nissu ?? -1,
                       receInf?.Kohi2Nissu ?? -1,
                       receInf?.Kohi3Nissu ?? -1,
                       receInf?.Kohi4Nissu ?? -1,
                       receInf?.Kohi1ReceKyufu ?? -1,
                       receInf?.Kohi2ReceKyufu ?? -1,
                       receInf?.Kohi3ReceKyufu ?? -1,
                       receInf?.Kohi4ReceKyufu ?? -1,
                       receInf?.HokenReceTensu ?? -1,
                       receInf?.HokenReceFutan ?? -1,
                       receInf?.Kohi1ReceTensu ?? -1,
                       receInf?.Kohi1ReceFutan ?? -1,
                       receInf?.Kohi2ReceTensu ?? -1,
                       receInf?.Kohi2ReceFutan ?? -1,
                       receInf?.Kohi3ReceTensu ?? -1,
                       receInf?.Kohi3ReceFutan ?? -1,
                       receInf?.Kohi4ReceTensu ?? -1,
                       receInf?.Kohi4ReceFutan ?? -1
                );
        }

        private List<SinKouiCountModel> GetSinKouiCounts(int hpId, long ptId, int sinYm, int hokenId)
        {
            var result = new List<SinKouiCountModel>();

            var sinKouis = NoTrackingDataContext.SinKouis.Where(p => p.HpId == hpId &&
                                                                     p.PtId == ptId &&
                                                                     p.SinYm == sinYm &&
                                                                     p.HokenId == hokenId &&
                                                                     p.IsNodspRece == 0 &&
                                                                     p.InoutKbn == 0 &&
                                                                     p.IsDeleted == DeleteTypes.None);

            var ptHokenPatterns = NoTrackingDataContext.PtHokenPatterns.Where(p => p.HpId == hpId &&
                                                                                                   p.PtId == ptId);

            var sinKouiJoinPatternQuery = from sinKoui in sinKouis
                                          join ptHokenPattern in ptHokenPatterns
                                          on sinKoui.HokenPid equals ptHokenPattern.HokenPid
                                          select new
                                          {
                                              sinKoui,
                                              ptHokenPattern
                                          };

            var sinKouiCounts = NoTrackingDataContext.SinKouiCounts.Where(p => p.HpId == hpId &&
                                                                          p.PtId == ptId &&
                                                                          p.SinYm == sinYm)
                                                                   .OrderBy(p => p.SinDate);

            var tenMsts = NoTrackingDataContext.TenMsts.Where(p => p.HpId == hpId && p.IsDeleted == DeleteTypes.None);

            var sinKouiDetails = NoTrackingDataContext.SinKouiDetails.Where(p => p.HpId == hpId &&
                                                                                 p.PtId == ptId &&
                                                                                 p.SinYm == sinYm &&
                                                                                 p.IsDeleted == DeleteTypes.None);

            var sinKouiJoinSinKouiCountquery = from sinKouiJoinPattern in sinKouiJoinPatternQuery
                                               join sinKouiCount in sinKouiCounts
                                               on new { sinKouiJoinPattern.sinKoui.RpNo, sinKouiJoinPattern.sinKoui.SeqNo } equals new { sinKouiCount.RpNo, sinKouiCount.SeqNo }
                                               select new
                                               {
                                                   sinKouiJoinPattern.ptHokenPattern,
                                                   SinKouiCount = sinKouiCount
                                               };

            var sinKouiCountJoinDetailQuery = from sinKouiJoinSinKouiCount in sinKouiJoinSinKouiCountquery
                                              join sinKouiDetail in sinKouiDetails
                                              on new { sinKouiJoinSinKouiCount.SinKouiCount.RpNo, sinKouiJoinSinKouiCount.SinKouiCount.SeqNo }
                                              equals new { sinKouiDetail.RpNo, sinKouiDetail.SeqNo }
                                              select new
                                              {
                                                  sinKouiJoinSinKouiCount.ptHokenPattern,
                                                  SinKouiCount = sinKouiJoinSinKouiCount.SinKouiCount,
                                                  SinKouiDetail = sinKouiDetail
                                              };

            var joinTenMstQuery = from sinKouiCountJoinDetail in sinKouiCountJoinDetailQuery
                                  join tenMst in tenMsts
                                  on sinKouiCountJoinDetail.SinKouiDetail.ItemCd equals tenMst.ItemCd into tempTenMstList
                                  select new
                                  {
                                      PtId = sinKouiCountJoinDetail.SinKouiCount.PtId,
                                      SinDate = sinKouiCountJoinDetail.SinKouiCount.SinDate,
                                      RaiinNo = sinKouiCountJoinDetail.SinKouiCount.RaiinNo,
                                      SinKouiCount = sinKouiCountJoinDetail.SinKouiCount,
                                      SinKouiDetail = sinKouiCountJoinDetail.SinKouiDetail,
                                      sinKouiCountJoinDetail.ptHokenPattern,
                                      TenMst = tempTenMstList.OrderByDescending(p => p.StartDate).FirstOrDefault(p => p.StartDate <= sinKouiCountJoinDetail.SinKouiCount.SinDate)
                                  };

            var groupKeys = joinTenMstQuery.GroupBy(p => new { p.PtId, p.SinDate, p.RaiinNo })
                                           .Select(p => p.FirstOrDefault());

            foreach (var groupKey in groupKeys)
            {
                var entities = joinTenMstQuery.Where(p => p.PtId == groupKey.PtId && p.SinDate == groupKey.SinDate && p.RaiinNo == groupKey.RaiinNo);
                var sinKouiDetailModels = new List<SinKouiDetailModel>();
                foreach (var entity in entities)
                {
                    sinKouiDetailModels.Add(new SinKouiDetailModel(entity.TenMst, entity.SinKouiDetail));
                }
                result.Add(new SinKouiCountModel(entities.Select(p => p.ptHokenPattern).Distinct().ToList(), groupKey.SinKouiCount, sinKouiDetailModels));
            }
            return result;
        }

        private List<ReceCheckErr> ClearReceCmtErr(int hpId, long ptId, int hokenId, int sinYm)
        {
            var oldReceCheckErrs = NoTrackingDataContext.ReceCheckErrs
                                                        .Where(p => p.HpId == hpId &&
                                                                    p.SinYm == sinYm &&
                                                                    p.PtId == ptId &&
                                                                    p.HokenId == hokenId)
                                                        .ToList();
            TrackingDataContext.ReceCheckErrs.RemoveRange(oldReceCheckErrs);

            return oldReceCheckErrs;
        }

        private void InsertReceCmtErr(int hpId, int userId, string userName, List<ReceCheckErr> oldReceCheckErrs, List<ReceCheckErr> newReceCheckErrs, ReceInfModel receInfModel, string errCd, string errMsg1, string errMsg2 = "", string aCd = " ", string bCd = " ", int sinDate = 0)
        {
            if (!string.IsNullOrEmpty(errMsg1) && errMsg1.Length > 100)
            {
                errMsg1 = CIUtil.Copy(errMsg1, 1, 99) + "…";
            }
            if (!string.IsNullOrEmpty(errMsg2) && errMsg2.Length > 100)
            {
                errMsg2 = CIUtil.Copy(errMsg2, 1, 99) + "…";
            }

            var existNewReceCheckErr = newReceCheckErrs.FirstOrDefault(p => p.HpId == hpId &&
                                           p.PtId == receInfModel.PtId &&
                                           p.SinYm == receInfModel.SinYm &&
                                           p.SinDate == sinDate &&
                                           p.HokenId == receInfModel.HokenId &&
                                           p.ErrCd == errCd &&
                                           p.ACd == aCd &&
                                           p.BCd == bCd);

            if (existNewReceCheckErr != null)
            {
                if (errCd == ReceErrCdConst.SanteiCountCheckErrCd)
                {
                    existNewReceCheckErr.Message1 = errMsg1;
                    existNewReceCheckErr.Message2 = errMsg2;
                }
                return;
            }

            var newReceCheckErr = new ReceCheckErr()
            {
                HpId = hpId,
                PtId = receInfModel.PtId,
                SinYm = receInfModel.SinYm,
                SinDate = sinDate,
                HokenId = receInfModel.HokenId,
                ACd = aCd,
                BCd = bCd,
                ErrCd = errCd,
                Message1 = errMsg1,
                Message2 = errMsg2,
                CreateId = userId,
                UpdateId = userId,
                CreateDate = CIUtil.GetJapanDateTimeNow(),
                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                CreateMachine = userName,
                UpdateMachine = userName
            };
            var existedReceCheckErr = oldReceCheckErrs.FirstOrDefault(p => p.HpId == newReceCheckErr.HpId &&
                                                                            p.PtId == newReceCheckErr.PtId &&
                                                                            p.SinYm == newReceCheckErr.SinYm &&
                                                                            p.SinDate == newReceCheckErr.SinDate &&
                                                                            p.HokenId == newReceCheckErr.HokenId &&
                                                                            p.ErrCd == newReceCheckErr.ErrCd &&
                                                                            p.ACd == newReceCheckErr.ACd &&
                                                                            p.BCd == newReceCheckErr.BCd);
            if (existedReceCheckErr != null)
            {
                newReceCheckErr.IsChecked = existedReceCheckErr.IsChecked;
            }
            newReceCheckErrs.Add(newReceCheckErr);
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
