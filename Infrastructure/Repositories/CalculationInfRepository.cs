using Domain.CalculationInf;
using Domain.Constant;
using Domain.Models.CalculationInf;
using Domain.Models.Diseases;
using Domain.Models.Medical;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Domain.Models.Receipt;
using Domain.Models.Receipt.Recalculation;
using Domain.Models.SystemConf;
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
        private List<BuiErrorModel> errorOdrInfDetails = new List<BuiErrorModel>();
        private readonly ISystemConfRepository _systemConfRepository;
        public CalculationInfRepository(ITenantProvider tenantProvider, ISystemConfRepository systemConfRepository) : base(tenantProvider)
        {
            _systemConfRepository = systemConfRepository;
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
                    CheckByomei(hpId, userId, userName, receInfModel, receCheckOpts, sinKouiCounts, oldReceCheckErrs, newReceCheckErrs);
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
                                InsertReceCmtErr(hpId, userId, userName, oldReceCheckErrs, newReceCheckErrs, receInfModel, ReceErrCdConst.ExpiredEndDateHokenErrCd,
                                    ReceErrCdConst.ExpiredEndDateHokenErrMsg, "（～" + CIUtil.SDateToShowSWDate(receInfModel.PtKohi1.EndDate) + "）", KOHI1_CHAR);
                            }
                        }
                    }
                    //kohi2
                    if (receInfModel.Kohi2Id > 0 && receInfModel.Kohi2Houbetu.AsInteger() != 102)
                    {
                        var firstSinKouiCount = sinKouiCounts.OrderBy(p => p.SinDate).FirstOrDefault(p => p.PtHokenPatterns
                            .Any(q => q.Kohi1Id == receInfModel.Kohi2Id || q.Kohi2Id == receInfModel.Kohi2Id || q.Kohi3Id == receInfModel.Kohi2Id || q.Kohi4Id == receInfModel.Kohi2Id));
                        if (firstSinKouiCount != null)
                        {
                            if (receInfModel.PtKohi2.StartDate > 0 && receInfModel.PtKohi2.StartDate > firstSinKouiCount.SinDate)
                            {
                                InsertReceCmtErr(hpId, userId, userName, oldReceCheckErrs, newReceCheckErrs, receInfModel, ReceErrCdConst.ExpiredStartDateHokenErrCd,
                                    ReceErrCdConst.ExpiredStartDateHokenErrMsg, "（" + CIUtil.SDateToShowSWDate(receInfModel.PtKohi2.StartDate) + "～）", KOHI2_CHAR);
                            }
                        }

                        var lastSinKouiCount = sinKouiCounts.OrderBy(p => p.SinDate).LastOrDefault(p => p.PtHokenPatterns
                            .Any(q => q.Kohi1Id == receInfModel.Kohi2Id || q.Kohi2Id == receInfModel.Kohi2Id || q.Kohi3Id == receInfModel.Kohi2Id || q.Kohi4Id == receInfModel.Kohi2Id));
                        if (lastSinKouiCount != null)
                        {
                            if (receInfModel.PtKohi2.EndDate > 0 && receInfModel.PtKohi2.EndDate < lastSinKouiCount.SinDate)
                            {
                                InsertReceCmtErr(hpId, userId, userName, oldReceCheckErrs, newReceCheckErrs, receInfModel, ReceErrCdConst.ExpiredEndDateHokenErrCd,
                                    ReceErrCdConst.ExpiredEndDateHokenErrMsg, "（～" + CIUtil.SDateToShowSWDate(receInfModel.PtKohi2.EndDate) + "）", KOHI2_CHAR);
                            }
                        }
                    }
                    //kohi3
                    if (receInfModel.Kohi3Id > 0 && receInfModel.Kohi3Houbetu.AsInteger() != 102)
                    {
                        var firstSinKouiCount = sinKouiCounts.OrderBy(p => p.SinDate).FirstOrDefault(p => p.PtHokenPatterns
                            .Any(q => q.Kohi1Id == receInfModel.Kohi3Id || q.Kohi2Id == receInfModel.Kohi3Id || q.Kohi3Id == receInfModel.Kohi3Id || q.Kohi4Id == receInfModel.Kohi3Id));
                        if (firstSinKouiCount != null)
                        {
                            if (receInfModel.PtKohi3.StartDate > 0 && receInfModel.PtKohi3.StartDate > firstSinKouiCount.SinDate)
                            {
                                InsertReceCmtErr(hpId, userId, userName, oldReceCheckErrs, newReceCheckErrs, receInfModel, ReceErrCdConst.ExpiredStartDateHokenErrCd,
                                    ReceErrCdConst.ExpiredStartDateHokenErrMsg, "（" + CIUtil.SDateToShowSWDate(receInfModel.PtKohi3.StartDate) + "～）", KOHI3_CHAR);
                            }
                        }

                        var lastSinKouiCount = sinKouiCounts.OrderBy(p => p.SinDate).LastOrDefault(p => p.PtHokenPatterns
                            .Any(q => q.Kohi1Id == receInfModel.Kohi3Id || q.Kohi2Id == receInfModel.Kohi3Id || q.Kohi3Id == receInfModel.Kohi3Id || q.Kohi4Id == receInfModel.Kohi3Id));
                        if (lastSinKouiCount != null)
                        {
                            if (receInfModel.PtKohi3.EndDate > 0 && receInfModel.PtKohi3.EndDate < lastSinKouiCount.SinDate)
                            {
                                InsertReceCmtErr(hpId, userId, userName, oldReceCheckErrs, newReceCheckErrs, receInfModel, ReceErrCdConst.ExpiredEndDateHokenErrCd,
                                     ReceErrCdConst.ExpiredEndDateHokenErrMsg, "（～" + CIUtil.SDateToShowSWDate(receInfModel.PtKohi3.EndDate) + "）", KOHI3_CHAR);
                            }
                        }
                    }
                    //kohi4
                    if (receInfModel.Kohi4Id > 0 && receInfModel.Kohi4Houbetu.AsInteger() != 102)
                    {
                        var firstSinKouiCount = sinKouiCounts.OrderBy(p => p.SinDate).FirstOrDefault(p => p.PtHokenPatterns
                            .Any(q => q.Kohi1Id == receInfModel.Kohi4Id || q.Kohi2Id == receInfModel.Kohi4Id || q.Kohi3Id == receInfModel.Kohi4Id || q.Kohi4Id == receInfModel.Kohi4Id));
                        if (firstSinKouiCount != null)
                        {
                            if (receInfModel.PtKohi4.StartDate > 0 && receInfModel.PtKohi4.StartDate > firstSinKouiCount.SinDate)
                            {
                                InsertReceCmtErr(hpId, userId, userName, oldReceCheckErrs, newReceCheckErrs, receInfModel, ReceErrCdConst.ExpiredStartDateHokenErrCd,
                                    ReceErrCdConst.ExpiredStartDateHokenErrMsg, "（" + CIUtil.SDateToShowSWDate(receInfModel.PtKohi4.StartDate) + "～）", KOHI4_CHAR);
                            }
                        }

                        var lastSinKouiCount = sinKouiCounts.OrderBy(p => p.SinDate).LastOrDefault(p => p.PtHokenPatterns
                            .Any(q => q.Kohi1Id == receInfModel.Kohi4Id || q.Kohi2Id == receInfModel.Kohi4Id || q.Kohi3Id == receInfModel.Kohi4Id || q.Kohi4Id == receInfModel.Kohi4Id));
                        if (lastSinKouiCount != null)
                        {
                            if (receInfModel.PtKohi4.EndDate > 0 && receInfModel.PtKohi4.EndDate < lastSinKouiCount.SinDate)
                            {
                                InsertReceCmtErr(hpId, userId, userName, oldReceCheckErrs, newReceCheckErrs, receInfModel, ReceErrCdConst.ExpiredEndDateHokenErrCd,
                                    ReceErrCdConst.ExpiredEndDateHokenErrMsg, "（～" + CIUtil.SDateToShowSWDate(receInfModel.PtKohi4.EndDate) + "）", KOHI4_CHAR);
                            }
                        }
                    }
                }
            }

            //E1003 unconfirmed
            if (receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.UnConfirmedHokenErrCd))
            {
                if (receInfModel.HokenId > 0 && !receInfModel.IsHokenConfirmed)
                {
                    string latestConfirmedDate = string.Empty;
                    if (receInfModel.HokenChecks?.Count > 0)
                    {
                        latestConfirmedDate = CIUtil.SDateToShowSWDate(receInfModel.HokenChecks.OrderByDescending(p => p.ConfirmDate).FirstOrDefault()?.ConfirmDate ?? 0);
                    }
                    InsertReceCmtErr(hpId, userId, userName, oldReceCheckErrs, newReceCheckErrs, receInfModel, ReceErrCdConst.UnConfirmedHokenErrCd,
                            ReceErrCdConst.UnConfirmedHokenErrMsg, "（最終確認: " + latestConfirmedDate + "）", HOKEN_CHAR);
                }
                if (receInfModel.Kohi1Id > 0 && !receInfModel.IsKohi1Confirmed)
                {
                    string latestConfirmedDate = string.Empty;
                    if (receInfModel.Kohi1Checks?.Count > 0)
                    {
                        latestConfirmedDate = CIUtil.SDateToShowSWDate(receInfModel.Kohi1Checks.OrderByDescending(p => p.ConfirmDate).FirstOrDefault()?.ConfirmDate ?? 0);
                    }
                    InsertReceCmtErr(hpId, userId, userName, oldReceCheckErrs, newReceCheckErrs, receInfModel, ReceErrCdConst.UnConfirmedHokenErrCd,
                             ReceErrCdConst.UnConfirmedHokenErrMsg, "（最終確認: " + latestConfirmedDate + "）", KOHI1_CHAR);
                }
                if (receInfModel.Kohi2Id > 0 && !receInfModel.IsKohi2Confirmed)
                {
                    string latestConfirmedDate = string.Empty;
                    if (receInfModel.Kohi2Checks?.Count > 0)
                    {
                        latestConfirmedDate = CIUtil.SDateToShowSWDate(receInfModel.Kohi2Checks.OrderByDescending(p => p.ConfirmDate).FirstOrDefault()?.ConfirmDate ?? 0);
                    }
                    InsertReceCmtErr(hpId, userId, userName, oldReceCheckErrs, newReceCheckErrs, receInfModel, ReceErrCdConst.UnConfirmedHokenErrCd,
                            ReceErrCdConst.UnConfirmedHokenErrMsg, "（最終確認: " + latestConfirmedDate + "）", KOHI2_CHAR);
                }
                if (receInfModel.Kohi3Id > 0 && !receInfModel.IsKohi3Confirmed)
                {
                    string latestConfirmedDate = string.Empty;
                    if (receInfModel.Kohi3Checks?.Count > 0)
                    {
                        latestConfirmedDate = CIUtil.SDateToShowSWDate(receInfModel.Kohi3Checks.OrderByDescending(p => p.ConfirmDate).FirstOrDefault()?.ConfirmDate ?? 0);
                    }
                    InsertReceCmtErr(hpId, userId, userName, oldReceCheckErrs, newReceCheckErrs, receInfModel, ReceErrCdConst.UnConfirmedHokenErrCd,
                            ReceErrCdConst.UnConfirmedHokenErrMsg, "（最終確認: " + latestConfirmedDate + "）", KOHI3_CHAR);
                }
                if (receInfModel.Kohi4Id > 0 && !receInfModel.IsKohi4Confirmed)
                {
                    string latestConfirmedDate = string.Empty;
                    if (receInfModel.Kohi4Checks?.Count > 0)
                    {
                        latestConfirmedDate = CIUtil.SDateToShowSWDate(receInfModel.Kohi4Checks.OrderByDescending(p => p.ConfirmDate).FirstOrDefault()?.ConfirmDate ?? 0);
                    }
                    InsertReceCmtErr(hpId, userId, userName, oldReceCheckErrs, newReceCheckErrs, receInfModel, ReceErrCdConst.UnConfirmedHokenErrCd,
                            ReceErrCdConst.UnConfirmedHokenErrMsg, "（最終確認: " + latestConfirmedDate + "）", KOHI4_CHAR);
                }
            }
        }

        private void CheckByomei(int hpId, int userId, string userName, ReceInfModel receInfModel, List<ReceCheckOptModel> receCheckOpts, List<SinKouiCountModel> sinKouiCounts, List<ReceCheckErr> oldReceCheckErrs, List<ReceCheckErr> newReceCheckErrs)
        {
            var ptByomeis = GetByomeiInThisMonth(hpId, receInfModel.SinYm, receInfModel.PtId, receInfModel.HokenId);
            if (ptByomeis.Count == 0)
            {
                //E2001 not exist byomei in month
                if (receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.NotExistByomeiErrCd))
                {
                    var sinKouiDetail = sinKouiCounts.FirstOrDefault(p => p.IsFirstVisit);
                    if (sinKouiDetail != null)
                    {
                        string msg2 = string.Format("（初診: {0}）", CIUtil.SDateToShowSWDate(sinKouiDetail.SinDate));
                        InsertReceCmtErr(hpId, userId, userName, oldReceCheckErrs, newReceCheckErrs, receInfModel, ReceErrCdConst.NotExistByomeiErrCd, ReceErrCdConst.NotExistByomeiErrMsg, msg2, sinDate: sinKouiDetail.SinDate);
                    }
                    else
                    {
                        InsertReceCmtErr(hpId, userId, userName, oldReceCheckErrs, newReceCheckErrs, receInfModel, ReceErrCdConst.NotExistByomeiErrCd, ReceErrCdConst.NotExistByomeiErrMsg);
                    }
                }

                //E2011 Bui Order Byomei
                if (_systemConfRepository.GetSettingValue(6003, 0, hpId) == 1 && receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.BuiOrderByomeiErrCd))
                {
                    foreach (var sinKouiCount in sinKouiCounts)
                    {
                        var odrInfs = GetOdrInfsBySinDate(hpId, receInfModel.PtId, sinKouiCount.SinDate, receInfModel.HokenId);
                        var buiOdrItemMsts = GetBuiOdrItemMsts(hpId);
                        var buiOdrItemByomeiMsts = GetBuiOdrItemByomeiMsts(hpId);
                        List<string> msgErrors = CheckBuiOrderByomei(buiOdrItemMsts, buiOdrItemByomeiMsts, odrInfs, ptByomeis);
                        if (msgErrors.Count > 0)
                        {
                            foreach (var msgError in msgErrors)
                            {

                                string itemName = odrInfs.FirstOrDefault(p => p.ItemCd == msgError)?.ItemName ?? string.Empty;
                                InsertReceCmtErr(hpId, userId, userName, oldReceCheckErrs, newReceCheckErrs, receInfModel, ReceErrCdConst.BuiOrderByomeiErrCd,
                                                                           ReceErrCdConst.BuiOrderByomeiErrMsg,
                                                                           itemName + " : " +
                                                                           CIUtil.SDateToShowSWDate(sinKouiCount.SinDate) + "）",
                                                                           msgError, sinDate: sinKouiCount.SinDate);
                            }
                        }
                    }
                }

                //E2010 Bui Order Byomei
                if (_systemConfRepository.GetSettingValue(6003, 0, hpId) == 1 && receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.ByomeiBuiOrderByomeiChekkuErrCd))
                {
                    var odrInfModels = GetOdrInfModels(hpId, receInfModel.PtId, receInfModel.SinYm, receInfModel.HokenId);
                    var buiOdrMsts = NoTrackingDataContext.BuiOdrMsts.Where(x => x.HpId == hpId);
                    var buiOdrByomeiMsts = NoTrackingDataContext.BuiOdrByomeiMsts.Where(x => x.HpId == hpId);
                    List<string> errorMsgs = CheckByomeiWithBuiOdr(odrInfModels, buiOdrMsts.ToList(), buiOdrByomeiMsts.ToList(), ptByomeis);
                    foreach (var errorOdrInfDetail in errorOdrInfDetails)
                    {
                        foreach (var msg in errorOdrInfDetail.Errors)
                        {
                            InsertReceCmtErr(hpId, userId, userName, oldReceCheckErrs, newReceCheckErrs, receInfModel, ReceErrCdConst.ByomeiBuiOrderByomeiChekkuErrCd, ReceErrCdConst.ByomeiBuiOrderByomeiChekkuErrMsg, msg, aCd: errorOdrInfDetail.ItemName, bCd: errorOdrInfDetail.OdrKouiKbn.ToString(), sinDate: errorOdrInfDetail.SinDate);
                        }
                    }
                }
            }
            else
            {

                //check if exist continous byomei in first visit or revisit
                if (receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.NotExistByomeiErrCd) || receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.CheckFirstVisit2003ByomeiErrCd))
                {
                    string format = string.Empty;
                    string msg2 = string.Empty;
                    foreach (var sinKouiDetail in sinKouiCounts)
                    {
                        //E2002 revisit
                        if (receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.NotExistByomeiErrCd) &&
                            sinKouiDetail.IsReVisit && !ptByomeis.Any(p => p.StartDate <= sinKouiDetail.SinDate &&
                            (p.TenkiDate >= sinKouiDetail.SinDate || p.IsContinous)))
                        {
                            format = "（再診: {0}）";
                            msg2 = string.Format(format, CIUtil.SDateToShowSWDate(sinKouiDetail.SinDate));
                            InsertReceCmtErr(hpId, userId, userName, oldReceCheckErrs, newReceCheckErrs, receInfModel, ReceErrCdConst.CheckReVisitContiByomeiErrCd, ReceErrCdConst.CheckReVisitContiByomeiErrMsg, msg2, sinDate: sinKouiDetail.SinDate);
                        }
                        //first visit
                        else if (receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.CheckFirstVisit2003ByomeiErrCd) && sinKouiDetail.IsFirstVisit)
                        {
                            //E2004
                            var checkedPtByomeis = new List<PtDiseaseModel>();
                            if (!sinKouiCounts.Any(p => p.SinDate == sinKouiDetail.SinDate && p.ExistSameFirstVisit))
                            {
                                foreach (var ptByomei in ptByomeis)
                                {
                                    if (ptByomei.StartDate < sinKouiDetail.SinDate && (ptByomei.TenkiDate >= sinKouiDetail.SinDate || ptByomei.IsContinous))
                                    {
                                        checkedPtByomeis.Add(ptByomei);
                                        format = "（初診: {0} {1}: {2}～）";
                                        string cutByomei = CIUtil.Copy(ptByomei.Byomei, 1, 100);
                                        msg2 = string.Format(format, CIUtil.SDateToShowSWDate(sinKouiDetail.SinDate),
                                                            cutByomei, CIUtil.SDateToShowSWDate(ptByomei.StartDate));
                                        InsertReceCmtErr(hpId, userId, userName, oldReceCheckErrs, newReceCheckErrs, receInfModel, ReceErrCdConst.CheckFirstVisit2003ByomeiErrCd, ReceErrCdConst.CheckFirstVisit2004ByomeiErrMsg,
                                                                         msg2, cutByomei, sinDate: sinKouiDetail.SinDate);
                                    }
                                }
                            }
                            //E2003
                            if (!ptByomeis.Any(p => !checkedPtByomeis.Contains(p) &&
                             p.StartDate <= sinKouiDetail.SinDate && (p.TenkiDate >= sinKouiDetail.SinDate || p.IsContinous)))
                            {
                                format = "（初診: {0}）";
                                msg2 = string.Format(format, CIUtil.SDateToShowSWDate(sinKouiDetail.SinDate));
                                InsertReceCmtErr(hpId, userId, userName, oldReceCheckErrs, newReceCheckErrs, receInfModel, ReceErrCdConst.CheckFirstVisit2003ByomeiErrCd, ReceErrCdConst.CheckFirstVisit2003ByomeiErrMsg, msg2, sinDate: sinKouiDetail.SinDate);
                            }
                        }
                    }
                }



                //E2005 check if has not main byomei 
                if (receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.HasNotMainByomeiErrCd) && !ptByomeis.Any(p => p.IsMain))
                {
                    InsertReceCmtErr(receInfModel, ReceErrCdConst.HasNotMainByomeiErrCd, ReceErrCdConst.HasNotMainByomeiErrMsg);
                }

                //E2006 check abandonment byomei
                if (receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.InvalidByomeiErrCd))
                {
                    foreach (var ptByomei in ptByomeis)
                    {
                        if (!ptByomei.IsFree && ptByomei.DelDate > 0 && ptByomei.DelDate < receInfModel.FirstDateOfThisMonth &&
                            (ptByomei.TenkiDate > ptByomei.DelDate || ptByomei.IsContinous))
                        {
                            string format = "（{0}: ～{1}）";
                            string msg2 = string.Format(format, ptByomei.Byomei, CIUtil.SDateToShowSWDate(ptByomei.DelDate));
                            InsertReceCmtErr(receInfModel, ReceErrCdConst.InvalidByomeiErrCd, ReceErrCdConst.InvalidByomeiErrMsg,
                                                                       msg2, ptByomei.ByomeiCd);
                        }
                    }
                }

                //E2007 check free text char count > 20
                if (receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.FreeTextLengthByomeiErrCd))
                {
                    foreach (var ptByomei in ptByomeis)
                    {
                        if (ptByomei.IsFree && ptByomei.Byomei.Length > 20)
                        {
                            string cutByomei = CIUtil.Copy(ptByomei.Byomei, 1, 100);
                            string msg2 = string.Format("({0}: {1}/20文字)", cutByomei, ptByomei.Byomei.Length);
                            InsertReceCmtErr(receInfModel, ReceErrCdConst.FreeTextLengthByomeiErrCd, ReceErrCdConst.FreeTextLengthByomeiErrMsg, msg2, cutByomei);
                        }
                    }
                }

                //E2008 check suspected byomei
                var receCheckOpt = receCheckOpts.FirstOrDefault(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.CheckSuspectedByomeiErrCd);
                if (receCheckOpt != null)
                {
                    foreach (var ptByomei in ptByomeis)
                    {
                        if (ptByomei.Byomei.AsString().Contains(SUSPECTED_SUFFIX) &&
                            CIUtil.DateTimeToInt(CIUtil.IntToDate(ptByomei.StartDate).AddMonths(receCheckOpt.CheckOpt)) <= receInfModel.LastDateOfThisMonth)
                        {
                            string format = "（{0}: {1}～）";
                            string cutByomei = CIUtil.Copy(ptByomei.Byomei, 1, 100);
                            string msg2 = string.Format(format, cutByomei, CIUtil.SDateToShowSWDate(ptByomei.StartDate));
                            InsertReceCmtErr(receInfModel, ReceErrCdConst.CheckSuspectedByomeiErrCd,
                                ReceErrCdConst.CheckSuspectedByomeiErrMsg.Replace("xx", receCheckOpt.CheckOpt.AsString()), msg2, cutByomei);
                        }
                    }
                }



                bool checkDuplicateByomei = receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.DuplicateByomeiCheckErrCd);
                bool checkDuplicateSyusyokuByomei = receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.DuplicateSyusyokuByomeiCheckErrCd);
                if (checkDuplicateByomei)
                {
                    var checkedByomeiList = new List<PtDiseaseModel>();
                    foreach (var ptByomei in ptByomeis)
                    {
                        if (ptByomei.IsFree || checkedByomeiList.Any(p => p.Id == ptByomei.Id))
                        {
                            continue;
                        }
                        foreach (var comparedPtByomei in ptByomeis)
                        {
                            if (comparedPtByomei.Id == ptByomei.Id || comparedPtByomei.IsFree)
                            {
                                continue;
                            }
                            bool isDuplicate = CheckDuplicateByomei(checkDuplicateByomei, checkDuplicateSyusyokuByomei, ptByomei, comparedPtByomei, receInfModel.HokenId);
                            if (isDuplicate)
                            {
                                checkedByomeiList.Add(ptByomei);
                                break;
                            }
                        }
                    }
                    foreach (var ptByomei in checkedByomeiList)
                    {
                        _commandHandler.InsertReceCmtErr(receInfModel, ReceErrCdConst.DuplicateByomeiCheckErrCd,
                                                                              ReceErrCdConst.DuplicateByomeiCheckErrMsg,
                                                                              "（" + ptByomei.Byomei + " : " + CIUtil.SDateToShowSWDate(ptByomei.StartDate) + "）",
                                                                              ptByomei.ByomeiCd, string.Join(string.Empty, ptByomei.GetAllSyusyokuCds().ToArray()));
                    }
                }



                bool checkByomeiResponding = _receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.HasNotByomeiWithOdrErrCd);
                bool checkBuiOrderByomei = _receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.BuiOrderByomeiErrCd);
                if (checkByomeiResponding || checkBuiOrderByomei)
                {
                    foreach (var sinKouiCount in _sinKouiCounts)
                    {
                        var odrInfs = _receByomeiCheckingFinder.GetOdrInfsBySinDate(receInfModel.PtId, sinKouiCount.SinDate, receInfModel.HokenId);

                        //E2009 check if exist byomei corresponding with order
                        if (checkByomeiResponding)
                        {
                            List<string> checkedItemCds = new List<string>();
                            foreach (var odrInf in odrInfs)
                            {
                                string itemCd = odrInf.ItemCd;
                                if (string.IsNullOrEmpty(itemCd) ||
                                    itemCd == ItemCdConst.Con_TouyakuOrSiBunkatu ||
                                    itemCd == ItemCdConst.Con_Refill) continue;

                                string santeiItemCd = _recalculationFinder.GetSanteiItemCd(itemCd, sinKouiCount.SinDate);

                                List<string> tekiouByomeiCds = _recalculationFinder.GetTekiouByomei(new List<string>() { itemCd, santeiItemCd });
                                if (tekiouByomeiCds.Count == 0) continue;

                                if (!ptByomeis.Where(p => p.StartDate <= odrInf.SinDate && (!odrInf.IsDrug || !p.Byomei.AsString().Contains(SUSPECTED_SUFFIX)))
                                             .Any(p => tekiouByomeiCds.Contains(p.ByomeiCd)))
                                {
                                    checkedItemCds.Add(odrInf.ItemCd);
                                    if (checkedItemCds.Count(p => p == odrInf.ItemCd) == 1)
                                    {
                                        _commandHandler.InsertReceCmtErr(receInfModel, ReceErrCdConst.HasNotByomeiWithOdrErrCd,
                                                                                   ReceErrCdConst.HasNotByomeiWithOdrErrMsg,
                                                                                   "（" + odrInf.ItemName + " : " +
                                                                                   CIUtil.SDateToShowSWDate(sinKouiCount.SinDate) + "）",
                                                                                   odrInf.ItemCd);
                                    }
                                }
                            }
                        }


                        //E2011 check bui order byomei
                        if (SystemConfig.Instance.VisibleBuiOrderCheck && checkBuiOrderByomei)
                        {
                            var buiOdrItemMsts = _recalculationFinder.GetBuiOdrItemMsts();
                            var buiOdrItemByomeiMsts = _recalculationFinder.GetBuiOdrItemByomeiMsts();
                            List<string> msgErrors = CheckBuiOrderByomei(buiOdrItemMsts, buiOdrItemByomeiMsts, odrInfs, ptByomeis);
                            if (msgErrors.Count > 0)
                            {
                                foreach (var msgError in msgErrors)
                                {
                                    string itemName = odrInfs.FirstOrDefault(p => p.ItemCd == msgError)?.ItemName;
                                    _commandHandler.InsertReceCmtErr(receInfModel, ReceErrCdConst.BuiOrderByomeiErrCd,
                                                                               ReceErrCdConst.BuiOrderByomeiErrMsg,
                                                                               itemName + " : " +
                                                                               CIUtil.SDateToShowSWDate(sinKouiCount.SinDate) + "）",
                                                                               msgError, sinDate: sinKouiCount.SinDate);
                                }
                            }
                        }
                    }
                }


                if (SystemConfig.Instance.VisibleBuiOrderCheck && _receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.ByomeiBuiOrderByomeiChekkuErrCd))
                {
                    var odrInfModels = _recalculationFinder.GetOdrInfModels(receInfModel.PtId, receInfModel.SinYm, receInfModel.HokenId);
                    var buiOdrMsts = _dbService.BuiOdrMstRepository.FindListNoTrack(x => x.HpId == hpId);
                    var buiOdrByomeiMsts = _dbService.BuiOdrByomeiMstRepository.FindListNoTrack(x => x.HpId == hpId);
                    List<string> errorMsgs = CheckByomeiWithBuiOdr(odrInfModels, buiOdrMsts, buiOdrByomeiMsts, ptByomeis);
                    foreach (var errorOdrInfDetail in errorOdrInfDetails)
                    {
                        foreach (var msg in errorOdrInfDetail.Errors)
                        {
                            _commandHandler.InsertReceCmtErr(receInfModel, ReceErrCdConst.ByomeiBuiOrderByomeiChekkuErrCd, ReceErrCdConst.ByomeiBuiOrderByomeiChekkuErrMsg, msg, aCd: errorOdrInfDetail.ItemName, bCd: errorOdrInfDetail.OdrKouiKbn.ToString(), sinDate: errorOdrInfDetail.SinDate);
                        }
                    }
                }

            }
        }

        private List<PtDiseaseModel> GetByomeiInThisMonth(int hpId, int sinYm, long ptId, int hokenId)
        {
            var result = new List<PtDiseaseModel>();
            int firstDateOfThisMonth = (sinYm + "01").AsInteger();
            int endDateOfThisMonth = (sinYm + "31").AsInteger();
            var ptByomeis = NoTrackingDataContext.PtByomeis.Where(p => p.HpId == hpId &&
                                                                                       p.PtId == ptId &&
                                                                                       p.IsDeleted == DeleteTypes.None &&
                                                                                       p.IsNodspRece == 0 &&
                                                                                       (p.TenkiKbn == TenkiKbnConst.Continued ||
                                                                                       p.StartDate <= endDateOfThisMonth && p.TenkiDate >= firstDateOfThisMonth) &&
                                                                                       (p.HokenPid == hokenId || p.HokenPid == 0));

            var byomeiMsts = NoTrackingDataContext.ByomeiMsts.Where(p => p.HpId == hpId);

            var query = from ptByomei in ptByomeis
                        join byomeiMst in byomeiMsts
                        on ptByomei.ByomeiCd equals byomeiMst.ByomeiCd into ByomeiMstList
                        from byomei in ByomeiMstList.DefaultIfEmpty()
                        select new
                        {
                            PtByomei = ptByomei,
                            ByomeiMst = byomei
                        };
            foreach (var entity in query)
            {
                result.Add(new PtDiseaseModel(entity.PtByomei, entity.ByomeiMst));
            }
            return result;
        }

        private int GetCountReceInfs(int hpId, List<long> ptIds, int sinYm)
        {
            return NoTrackingDataContext.ReceInfs.Count(p => p.HpId == hpId && p.SeikyuYm == sinYm && (ptIds.Count == 0 || ptIds.Contains(p.PtId)));
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

        private List<OrdInfDetailModel> GetOdrInfsBySinDate(int hpId, long ptId, int sinDate, int hokenId)
        {
            var result = new List<OrdInfDetailModel>();

            var hokenPids = NoTrackingDataContext.PtHokenPatterns.Where(p => p.HpId == hpId &&
                                                                                              p.PtId == ptId &&
                                                                                              p.HokenId == hokenId &&
                                                                                              p.IsDeleted == DeleteTypes.None)
                                                                        .Select(p => p.HokenPid).ToList();

            var tenMstQuery = NoTrackingDataContext.TenMsts.Where(p => p.HpId == hpId);

            var odrInfDetails = NoTrackingDataContext.OdrInfDetails.Where(odrDetail => odrDetail.HpId == hpId &&
                                                                                       odrDetail.PtId == ptId &&
                                                                                       odrDetail.SinDate == sinDate);

            var odrInfs = NoTrackingDataContext.OdrInfs.Where(odr => odr.HpId == hpId &&
                                                                     odr.PtId == ptId &&
                                                                     odr.SinDate == sinDate &&
                                                                     odr.SanteiKbn == 0 &&
                                                                     hokenPids.Contains(odr.HokenPid) &&
                                                                     odr.IsDeleted == DeleteTypes.None &&
                                                                     odr.OdrKouiKbn != 10);

            var odrInfJoinDetailQuery = from odrInf in odrInfs
                                        join odrInfDetail in odrInfDetails
                                        on new { odrInf.RaiinNo, odrInf.RpNo, odrInf.RpEdaNo }
                                        equals new { odrInfDetail.RaiinNo, odrInfDetail.RpNo, odrInfDetail.RpEdaNo }
                                        into tempOdrDetails
                                        select new
                                        {
                                            OdrInf = odrInf,
                                            OdrInfDetails = tempOdrDetails,
                                        };
            var odrInfJoinDetails = odrInfJoinDetailQuery.ToList();
            foreach (var odrInfJoinDetail in odrInfJoinDetails)
            {
                result.AddRange(odrInfJoinDetail.OdrInfDetails.Select(p => new OrdInfDetailModel(p)));
            }
            return result;
        }

        private List<BuiOdrItemMst> GetBuiOdrItemMsts(int hpId)
        {
            return NoTrackingDataContext.BuiOdrItemMsts.Where(x => x.HpId == hpId).ToList();
        }

        private List<BuiOdrItemByomeiMst> GetBuiOdrItemByomeiMsts(int hpId)
        {
            return NoTrackingDataContext.BuiOdrItemByomeiMsts.Where(x => x.HpId == hpId).ToList();
        }

        private List<string> CheckBuiOrderByomei(List<BuiOdrItemMst> buiOdrItemMsts, List<BuiOdrItemByomeiMst> buiOdrItemByomeiMsts, List<OrdInfDetailModel> todayOrderInfModels, List<PtDiseaseModel> ptByomeiModels)
        {
            List<string> msgErrors = new List<string>();
            foreach (var todayOrderInfModel in todayOrderInfModels)
            {
                if (!buiOdrItemMsts.Any(p => p.ItemCd == todayOrderInfModel.ItemCd)) continue;

                var buiOdrByomeiMsts = buiOdrItemByomeiMsts.FindAll(p => p.ItemCd == todayOrderInfModel.ItemCd);
                if (buiOdrByomeiMsts.Count > 0)
                {
                    bool hasError = true;
                    foreach (var buiOdrByomeiMst in buiOdrByomeiMsts)
                    {
                        if (buiOdrByomeiMst.LrKbn == 0 && buiOdrByomeiMst.BothKbn == 0)
                        {
                            if (ptByomeiModels.Any(p => buiOdrByomeiMsts.Any(q => p.ByomeiHankToZen.Contains(HenkanJ.Instance.ToFullsize(q.ByomeiBui)))))
                            {
                                hasError = false;
                                break;
                            }
                        }
                        else if (buiOdrByomeiMst.LrKbn == 1 && buiOdrByomeiMst.BothKbn == 1)
                        {
                            if (ptByomeiModels.Any(p => (p.ByomeiHankToZen.AsString().Contains(LEFT) || p.ByomeiHankToZen.AsString().Contains(RIGHT) ||
                                p.ByomeiHankToZen.AsString().Contains(BOTH)) && buiOdrByomeiMsts.Any(q => p.ByomeiHankToZen.Contains(HenkanJ.Instance.ToFullsize(q.ByomeiBui)))))
                            {
                                hasError = false;
                                break;
                            }
                        }
                        else if (buiOdrByomeiMst.LrKbn == 1 && buiOdrByomeiMst.BothKbn == 0)
                        {
                            if (ptByomeiModels.Any(p => (p.ByomeiHankToZen.AsString().Contains(LEFT) || p.ByomeiHankToZen.AsString().Contains(RIGHT))
                                && !p.ByomeiHankToZen.AsString().Contains(LEFT_RIGHT) && !p.ByomeiHankToZen.AsString().Contains(RIGHT_LEFT) && buiOdrByomeiMsts.Any(q => p.Byomei.Contains(HenkanJ.Instance.ToFullsize(q.ByomeiBui)))))
                            {
                                hasError = false;
                                break;
                            }
                        }
                        else if (buiOdrByomeiMst.LrKbn == 0 && buiOdrByomeiMst.BothKbn == 1)
                        {
                            if (ptByomeiModels.Any(p => (p.ByomeiHankToZen.AsString().Contains(BOTH) || p.ByomeiHankToZen.AsString().Contains(LEFT_RIGHT) || p.ByomeiHankToZen.AsString().Contains(RIGHT_LEFT))
                                && buiOdrByomeiMsts.Any(q => p.Byomei.Contains(HenkanJ.Instance.ToFullsize(q.ByomeiBui)))))
                            {
                                hasError = false;
                                break;
                            }
                        }

                    }
                    if (hasError)
                    {
                        msgErrors.Add(todayOrderInfModel.ItemCd);
                    }
                }
            }

            return msgErrors;
        }

        public List<OrdInfModel> GetOdrInfModels(int hpId, long ptId, int sinYm, int hokenId)
        {
            List<OrdInfModel> result = new List<OrdInfModel>();

            List<int> hokenPIds = NoTrackingDataContext.SinKouis.Where(p => p.HpId == hpId &&
                                                                            p.PtId == ptId &&
                                                                            p.SinYm == sinYm &&
                                                                            p.HokenId == hokenId &&
                                                                            p.IsNodspRece == 0 &&
                                                                            p.IsDeleted == DeleteTypes.None)
                                                             .Select(p => p.HokenPid).Distinct().ToList();

            var tenMstQuery = NoTrackingDataContext.TenMsts.Where(p => p.HpId == hpId);

            var odrInfDetails = NoTrackingDataContext.OdrInfDetails.Where(odrDetail => odrDetail.HpId == hpId &&
                                                                                                odrDetail.PtId == ptId);

            var detailJoinTenMstQuery = from odrDetail in odrInfDetails
                                        join tenMst in tenMstQuery on odrDetail.ItemCd equals tenMst.ItemCd into tempTenMsts
                                        select new
                                        {
                                            OdrDetail = odrDetail,
                                            TenMsts = tempTenMsts
                                        };
            var odrInfs = NoTrackingDataContext.OdrInfs.Where(odr => odr.HpId == hpId &&
                                                                     odr.PtId == ptId &&
                                                                     odr.SinDate / 100 == sinYm &&
                                                                     hokenPIds.Contains(odr.HokenPid) &&
                                                                     odr.IsDeleted == DeleteTypes.None &&
                                                                     odr.OdrKouiKbn != 10);
            var odrInfJoinDetailQuery = from odrInf in odrInfs
                                        join detailJoinTenMst in detailJoinTenMstQuery
                                        on new { odrInf.RaiinNo, odrInf.SinDate, odrInf.RpNo, odrInf.RpEdaNo }
                                        equals new { detailJoinTenMst.OdrDetail.RaiinNo, detailJoinTenMst.OdrDetail.SinDate, detailJoinTenMst.OdrDetail.RpNo, detailJoinTenMst.OdrDetail.RpEdaNo }
                                        into tempOdrDetails
                                        select new
                                        {
                                            OdrInf = odrInf,
                                            OdrInfDetails = tempOdrDetails,
                                        };
            var odrInfJoinDetails = odrInfJoinDetailQuery.ToList();
            foreach (var odrInfJoinDetail in odrInfJoinDetails)
            {
                var odrInfDetailModels = new List<OrdInfDetailModel>();
                foreach (var odrInfDetail in odrInfJoinDetail.OdrInfDetails)
                {
                    odrInfDetailModels.Add(new OrdInfDetailModel(odrInfDetail.OdrDetail, odrInfDetail.TenMsts.ToList()));
                }
                result.Add(new OrdInfModel(odrInfJoinDetail.OdrInf, odrInfDetailModels));
            }

            return result;
        }

        public List<string> CheckByomeiWithBuiOdr(List<OrdInfModel> odrInfModels, List<BuiOdrMst> buiOdrMsts, List<BuiOdrByomeiMst> buiOdrByomeiMsts, List<PtDiseaseModel> ptByomeis)
        {
            bool IsSpecialComment(OrdInfDetailModel detail)
            {
                return detail.SinKouiKbn == 99 && !string.IsNullOrEmpty(detail.CmtOpt);
            }

            errorOdrInfDetails = new List<BuiErrorModel>();
            List<string> errorMsgs = new List<string>();
            foreach (var odrInf in odrInfModels)
            {
                var odrInfDetailModels = odrInf.OrdInfDetails.Where(x => string.IsNullOrEmpty(x.ItemCd) || x.ItemCd.Length == 4 || x.SinKouiKbn == 99);
                foreach (var detail in odrInfDetailModels)
                {

                    var buiOdrMstCheckList = new List<BuiOdrMst>();
                    var filteredBuiOdrMsts = new List<BuiOdrMst>();
                    string compareName = IsSpecialComment(detail) ? detail.ItemName.Replace(detail.CmtName, "") : detail.ItemName;
                    compareName = HenkanJ.Instance.ToFullsize(compareName);
                    List<BuiOdrMst> buiOdrMstContainItemNames = new List<BuiOdrMst>();
                    foreach (var buiOdrMst in buiOdrMsts)
                    {
                        List<string> odrBuiPatterns = new List<string>();
                        if (buiOdrMst.MustLrKbn == 1)
                        {
                            if (buiOdrMst.LrKbn == 1 && buiOdrMst.BothKbn == 1)
                            {
                                odrBuiPatterns.Add($"{BOTH}{buiOdrMst.OdrBui}");
                                odrBuiPatterns.Add($"{LEFT}{buiOdrMst.OdrBui}");
                                odrBuiPatterns.Add($"{RIGHT}{buiOdrMst.OdrBui}");
                                odrBuiPatterns.Add($"{LEFT_RIGHT}{buiOdrMst.OdrBui}");
                                odrBuiPatterns.Add($"{RIGHT_LEFT}{buiOdrMst.OdrBui}");
                            }
                            else if (buiOdrMst.LrKbn == 1 && buiOdrMst.BothKbn == 0)
                            {
                                odrBuiPatterns.Add($"{LEFT}{buiOdrMst.OdrBui}");
                                odrBuiPatterns.Add($"{RIGHT}{buiOdrMst.OdrBui}");
                            }
                            else if (buiOdrMst.LrKbn == 0 && buiOdrMst.BothKbn == 1)
                            {
                                odrBuiPatterns.Add($"{BOTH}{buiOdrMst.OdrBui}");
                                odrBuiPatterns.Add($"{LEFT_RIGHT}{buiOdrMst.OdrBui}");
                                odrBuiPatterns.Add($"{RIGHT_LEFT}{buiOdrMst.OdrBui}");
                            }
                        }
                        else
                        {
                            odrBuiPatterns.Add(buiOdrMst.OdrBui);
                        }
                        foreach (var pattern in odrBuiPatterns)
                        {
                            if (compareName.Contains(HenkanJ.Instance.ToFullsize(pattern)))
                            {
                                buiOdrMstContainItemNames.Add(buiOdrMst);
                                break;
                            }
                        }
                    }

                    if (30 <= odrInf.OdrKouiKbn && odrInf.OdrKouiKbn <= 39)
                    {
                        buiOdrMstCheckList.AddRange(buiOdrMstContainItemNames.Where(mst => mst.Koui30 == 1));
                    }
                    else if (40 <= odrInf.OdrKouiKbn && odrInf.OdrKouiKbn <= 49)
                    {
                        buiOdrMstCheckList.AddRange(buiOdrMstContainItemNames.Where(mst => mst.Koui40 == 1));
                    }
                    else if (50 <= odrInf.OdrKouiKbn && odrInf.OdrKouiKbn <= 59)
                    {
                        buiOdrMstCheckList.AddRange(buiOdrMstContainItemNames.Where(mst => mst.Koui50 == 1));
                    }
                    else if (60 <= odrInf.OdrKouiKbn && odrInf.OdrKouiKbn <= 69)
                    {
                        buiOdrMstCheckList.AddRange(buiOdrMstContainItemNames.Where(mst => mst.Koui60 == 1));
                    }
                    else if (70 <= odrInf.OdrKouiKbn && odrInf.OdrKouiKbn <= 79)
                    {
                        buiOdrMstCheckList.AddRange(buiOdrMstContainItemNames.Where(mst => mst.Koui70 == 1));
                    }
                    else if (80 <= odrInf.OdrKouiKbn && odrInf.OdrKouiKbn <= 89)
                    {
                        buiOdrMstCheckList.AddRange(buiOdrMstContainItemNames.Where(mst => mst.Koui80 == 1));
                    }

                    var buiOdrMstWithMaxLength = buiOdrMstCheckList.OrderByDescending(x => x.OdrBui.Length).FirstOrDefault();
                    if (buiOdrMstWithMaxLength == null) continue;
                    filteredBuiOdrMsts.Add(buiOdrMstWithMaxLength);
                    var buiOdrMstsWithSameLength = buiOdrMstCheckList.Where(x => x.OdrBui.Length == buiOdrMstWithMaxLength.OdrBui.Length && x != buiOdrMstWithMaxLength);
                    filteredBuiOdrMsts.AddRange(buiOdrMstsWithSameLength);

                    foreach (var buiOdrMst in filteredBuiOdrMsts)
                    {
                        bool isValid = false;
                        var filteredBuiOdrByomeiMsts = buiOdrByomeiMsts.Where(mst => mst.BuiId == buiOdrMst.BuiId);
                        var ptByomeisContainByomeiBui = new List<PtDiseaseModel>();
                        foreach (var ptByomei in ptByomeis)
                        {
                            foreach (var mst in filteredBuiOdrByomeiMsts)
                            {
                                if (HenkanJ.Instance.ToFullsize(ptByomei.Byomei).Contains(HenkanJ.Instance.ToFullsize(mst.ByomeiBui)))
                                {
                                    ptByomeisContainByomeiBui.Add(ptByomei);
                                    break;
                                }
                            }
                        }
                        foreach (var ptByomei in ptByomeisContainByomeiBui)
                        {
                            isValid = ValidateByomeiReflectOdrSite(compareName, HenkanJ.Instance.ToFullsize(ptByomei.Byomei), buiOdrMst.LrKbn, buiOdrMst.BothKbn);
                            if (isValid) break;
                        }
                        if (!isValid)
                        {
                            string format = "（{0}／{1}：{2}）";
                            string output = IsSpecialComment(detail) ? detail.ItemName.Replace(detail.CmtName, "") : detail.ItemName;
                            string msg2 = string.Format(format, OdrKouiKbnToString(odrInf.OdrKouiKbn), output, CIUtil.SDateToShowSWDate(odrInf.SinDate));
                            errorMsgs.Add(msg2);
                            if (!errorOdrInfDetails.Any(d => d.OdrInfDetail == detail))
                            {
                                errorOdrInfDetails.Add(new BuiErrorModel(detail, odrInf.OdrKouiKbn, odrInf.SinDate, output));
                            }
                            errorOdrInfDetails.First(x => x.OdrInfDetail == detail).Errors.Add(msg2);
                        }
                    }
                }
            }
            return errorMsgs;
        }

        public bool ValidateByomeiReflectOdrSite(string buiOdr, string byomeiName, int LrKbn, int BothKbn)
        {
            string GetDirection(string name)
            {
                string str = name.Length >= 2 ? name.Substring(0, 2) : name;
                if (str.Contains(BOTH))
                {
                    return BOTH;
                }
                else if (str == $"{LEFT}{RIGHT}" || str == $"{RIGHT}{LEFT}")
                {
                    return str;
                }
                else if (str.Contains(LEFT))
                {
                    return LEFT;
                }
                else if (str.Contains(RIGHT))
                {
                    return RIGHT;
                }
                return "";
            }
            if (LrKbn == 0 && BothKbn == 0)
            {
                return true;
            }
            else if ((LrKbn == 1 && BothKbn == 1) || (LrKbn == 0 && BothKbn == 1))
            {
                string buiOdrDirection = GetDirection(buiOdr);
                string byomeiNameDirection = GetDirection(byomeiName);
                // Convert names to the left-right direction if they contain 両 character or right-left direction.
                string buiOdrLeftRight = buiOdrDirection.Replace($"{BOTH}", $"{LEFT}{RIGHT}").Replace($"{RIGHT}{LEFT}", $"{LEFT}{RIGHT}");
                string byomeiNameLeftRight = byomeiNameDirection.Replace($"{BOTH}", $"{LEFT}{RIGHT}").Replace($"{RIGHT}{LEFT}", $"{LEFT}{RIGHT}");
                return byomeiNameLeftRight.Contains(buiOdrLeftRight);
            }
            else if (LrKbn == 1 && BothKbn == 0)
            {
                string buiOdrDirection = GetDirection(buiOdr);
                string byomeiNameDirection = GetDirection(byomeiName);
                // Convert names to the left-right direction if they contain 両 character or right-left direction.
                string buiOdrLeftRight = buiOdrDirection.Replace($"{BOTH}", $"{LEFT}{RIGHT}").Replace($"{RIGHT}{LEFT}", $"{LEFT}{RIGHT}");
                string byomeiNameLeftRight = byomeiNameDirection.Replace($"{BOTH}", $"{LEFT}{RIGHT}").Replace($"{RIGHT}{LEFT}", $"{LEFT}{RIGHT}");
                if (byomeiNameLeftRight.Contains($"{LEFT}{RIGHT}") && (buiOdrLeftRight == LEFT || buiOdrLeftRight == RIGHT))
                {
                    return false;
                }
                return byomeiNameLeftRight.Contains(buiOdrLeftRight);
            }
            return true;
        }

        private string OdrKouiKbnToString(int odrKouiKbn)
        {
            if (30 <= odrKouiKbn && odrKouiKbn <= 39)
            {
                return "注射";
            }
            else if (40 <= odrKouiKbn && odrKouiKbn <= 49)
            {
                return "処置";
            }
            else if (50 <= odrKouiKbn && odrKouiKbn <= 59)
            {
                return "手術";
            }
            else if (60 <= odrKouiKbn && odrKouiKbn <= 69)
            {
                return "検査";
            }
            else if (70 <= odrKouiKbn && odrKouiKbn <= 79)
            {
                return "画像";
            }
            else if (80 <= odrKouiKbn && odrKouiKbn <= 89)
            {
                return "その他";
            }
            return "";
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
