using CommonChecker.Models.OrdInf;
using CommonChecker.Models.OrdInfDetailModel;
using CommonCheckers.OrderRealtimeChecker.Models;
using Domain.CalculationInf;
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
using Interactor.CalculateService;
using Interactor.CommonChecker.CommonMedicalCheck;
using UseCase.MedicalExamination.Calculate;
using UseCase.ReceiptCheck;
using Request = UseCase.Receipt.Recalculation;

namespace Interactor.ReceiptCheck
{
    public class RecalculationInteractor : IRecalculationInputPort
    {
        public const string HOKEN_CHAR = "0";
        public const string KOHI1_CHAR = "1";
        public const string KOHI2_CHAR = "2";
        public const string KOHI3_CHAR = "3";
        public const string KOHI4_CHAR = "4";
        public const string FREE_WORD = "0000999";
        public const string SUSPECTED_SUFFIX = "の疑い";
        public const string LEFT = "左";
        public const string RIGHT = "右";
        public const string BOTH = "両";
        public const string LEFT_RIGHT = "左右";
        public const string RIGHT_LEFT = "右左";

        private readonly ICalculateService _calculateService;
        private readonly ICalculationInfRepository _calculationInfRepository;
        private readonly ISystemConfRepository _systemConfRepository;
        private readonly ICommonMedicalCheck _commonMedicalCheck;

        private List<ReceInfModel> _receInfModels;
        private List<ReceCheckOptModel> _receCheckOpts;
        private List<SinKouiCountModel> _sinKouiCounts;
        private List<ReceCheckErrModel> _newReceCheckErrs;
        private List<ReceCheckErrModel> _oldReceCheckErrs;
        public List<BuiErrorModel> errorOdrInfDetails = new List<BuiErrorModel>();

        public RecalculationInteractor(ICalculateService calculateService, ICalculationInfRepository calculationInfRepository, ISystemConfRepository systemConfRepository, ICommonMedicalCheck commonMedicalCheck)
        {
            _calculateService = calculateService;
            _calculationInfRepository = calculationInfRepository;
            _systemConfRepository = systemConfRepository;
            _commonMedicalCheck = commonMedicalCheck;
        }

        public RecalculationOutputData Handle(RecalculationInputData inputData)
        {
            _calculateService.RunCalculateMonth(
                new Request.CalculateMonthRequest()
                {
                    HpId = inputData.HpId,
                    SeikyuYm = inputData.SeikyuYm,
                    PtIds = inputData.PtIds,
                    PreFix = ""
                });

            _calculateService.ReceFutanCalculateMain(new ReceCalculateRequest(inputData.PtIds, inputData.SeikyuYm));

            CheckErrorInMonth(inputData.HpId, inputData.SeikyuYm, inputData.PtIds);
        }

        public void CheckErrorInMonth(int hpId, int userId, string userName, int seikyuYm, List<long> ptIds)
        {
            IsStopCalc = false;
            AllCheckCount = _calculationInfRepository.GetCountReceInfs(hpId, ptIds, seikyuYm);
            CheckedCount = 0;

            _receCheckOpts = _calculationInfRepository.GetReceCheckOpts(hpId);
            _receInfModels = _calculationInfRepository.GetReceInfModels(hpId, ptIds, seikyuYm);

            foreach (var receInfModel in _receInfModels)
            {
                if (IsStopCalc) break;
                if (CancellationToken.IsCancellationRequested) return;
                _commandHandler.ClearReceCmtErr(receInfModel.PtId, receInfModel.HokenId, receInfModel.SinYm);
                _sinKouiCounts = _calculationInfRepository.GetSinKouiCounts(hpId, receInfModel.PtId, receInfModel.SinYm, receInfModel.HokenId);

                CheckHoken(hpId, userId, userName, receInfModel);
                CheckByomei(hpId, userId, userName, receInfModel);
                CheckOrder(hpId, userId, userName, receInfModel);
                CheckRosai(hpId, userId, userName, receInfModel);
                CheckAftercare(hpId, userId, userName, receInfModel);

                CheckedCount++;
            }
            _commandHandler.SaveChanged();
            PrintReceCheck(seikyuYm, ptIds);
        }

        public void CheckHoken(int hpId, int userId, string userName, ReceInfModel receInfModel)
        {
            //expired
            if (_receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.ExpiredEndDateHokenErrCd))
            {
                if (_sinKouiCounts.Count > 0)
                {
                    //hoken
                    if (receInfModel.HokenId > 0 && receInfModel.Houbetu.AsInteger() != 0)
                    {
                        //E1002 start date
                        var firstSinKouiCount = _sinKouiCounts.OrderBy(p => p.SinDate).FirstOrDefault(p => p.PtHokenPatterns.Any(q => q.HokenId == receInfModel.HokenId));
                        if (firstSinKouiCount != null)
                        {
                            if (receInfModel.PtHokenInf.StartDate > 0 && receInfModel.PtHokenInf.StartDate > firstSinKouiCount.SinDate)
                            {
                                _calculationInfRepository.InsertReceCmtErr(hpId, userId, userName, _oldReceCheckErrs, _newReceCheckErrs, receInfModel, ReceErrCdConst.ExpiredStartDateHokenErrCd,
                                       ReceErrCdConst.ExpiredStartDateHokenErrMsg, "（" + CIUtil.SDateToShowSWDate(receInfModel.PtHokenInf.StartDate) + "～）", HOKEN_CHAR);
                            }
                        }

                        //E1001 end date
                        var lastSinKouiCount = _sinKouiCounts.OrderBy(p => p.SinDate).LastOrDefault(p => p.PtHokenPatterns.Any(q => q.HokenId == receInfModel.HokenId));
                        if (lastSinKouiCount != null)
                        {
                            if (receInfModel.PtHokenInf.EndDate > 0 && receInfModel.PtHokenInf.EndDate < lastSinKouiCount.SinDate)
                            {
                                _calculationInfRepository.InsertReceCmtErr(hpId, userId, userName, _oldReceCheckErrs, _newReceCheckErrs, receInfModel, ReceErrCdConst.ExpiredEndDateHokenErrCd,
                                    ReceErrCdConst.ExpiredEndDateHokenErrMsg, "（～" + CIUtil.SDateToShowSWDate(receInfModel.PtHokenInf.EndDate) + "）", HOKEN_CHAR);
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
                            if (receInfModel.PtKohi1.StartDate > 0 && receInfModel.PtKohi1.StartDate > firstSinKouiCount.SinDate)
                            {
                                _calculationInfRepository.InsertReceCmtErr(hpId, userId, userName, _oldReceCheckErrs, _newReceCheckErrs, receInfModel, ReceErrCdConst.ExpiredStartDateHokenErrCd,
                                    ReceErrCdConst.ExpiredStartDateHokenErrMsg, "（" + CIUtil.SDateToShowSWDate(receInfModel.PtKohi1.StartDate) + "～）", KOHI1_CHAR);
                            }
                        }

                        var lastSinKouiCount = _sinKouiCounts.OrderBy(p => p.SinDate).LastOrDefault(p => p.PtHokenPatterns
                            .Any(q => q.Kohi1Id == receInfModel.Kohi1Id || q.Kohi2Id == receInfModel.Kohi1Id || q.Kohi3Id == receInfModel.Kohi1Id || q.Kohi4Id == receInfModel.Kohi1Id));
                        if (lastSinKouiCount != null)
                        {
                            if (receInfModel.PtKohi1.EndDate > 0 && receInfModel.PtKohi1.EndDate < lastSinKouiCount.SinDate)
                            {
                                _calculationInfRepository.InsertReceCmtErr(hpId, userId, userName, _oldReceCheckErrs, _newReceCheckErrs, receInfModel, ReceErrCdConst.ExpiredEndDateHokenErrCd,
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
                            if (receInfModel.PtKohi2.StartDate > 0 && receInfModel.PtKohi2.StartDate > firstSinKouiCount.SinDate)
                            {
                                _calculationInfRepository.InsertReceCmtErr(hpId, userId, userName, _oldReceCheckErrs, _newReceCheckErrs, receInfModel, ReceErrCdConst.ExpiredStartDateHokenErrCd,
                                    ReceErrCdConst.ExpiredStartDateHokenErrMsg, "（" + CIUtil.SDateToShowSWDate(receInfModel.PtKohi2.StartDate) + "～）", KOHI2_CHAR);
                            }
                        }

                        var lastSinKouiCount = _sinKouiCounts.OrderBy(p => p.SinDate).LastOrDefault(p => p.PtHokenPatterns
                            .Any(q => q.Kohi1Id == receInfModel.Kohi2Id || q.Kohi2Id == receInfModel.Kohi2Id || q.Kohi3Id == receInfModel.Kohi2Id || q.Kohi4Id == receInfModel.Kohi2Id));
                        if (lastSinKouiCount != null)
                        {
                            if (receInfModel.PtKohi2.EndDate > 0 && receInfModel.PtKohi2.EndDate < lastSinKouiCount.SinDate)
                            {
                                _calculationInfRepository.InsertReceCmtErr(hpId, userId, userName, _oldReceCheckErrs, _newReceCheckErrs, receInfModel, ReceErrCdConst.ExpiredEndDateHokenErrCd,
                                    ReceErrCdConst.ExpiredEndDateHokenErrMsg, "（～" + CIUtil.SDateToShowSWDate(receInfModel.PtKohi2.EndDate) + "）", KOHI2_CHAR);
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
                            if (receInfModel.PtKohi3.StartDate > 0 && receInfModel.PtKohi3.StartDate > firstSinKouiCount.SinDate)
                            {
                                _calculationInfRepository.InsertReceCmtErr(hpId, userId, userName, _oldReceCheckErrs, _newReceCheckErrs, receInfModel, ReceErrCdConst.ExpiredStartDateHokenErrCd,
                                    ReceErrCdConst.ExpiredStartDateHokenErrMsg, "（" + CIUtil.SDateToShowSWDate(receInfModel.PtKohi3.StartDate) + "～）", KOHI3_CHAR);
                            }
                        }

                        var lastSinKouiCount = _sinKouiCounts.OrderBy(p => p.SinDate).LastOrDefault(p => p.PtHokenPatterns
                            .Any(q => q.Kohi1Id == receInfModel.Kohi3Id || q.Kohi2Id == receInfModel.Kohi3Id || q.Kohi3Id == receInfModel.Kohi3Id || q.Kohi4Id == receInfModel.Kohi3Id));
                        if (lastSinKouiCount != null)
                        {
                            if (receInfModel.PtKohi3.EndDate > 0 && receInfModel.PtKohi3.EndDate < lastSinKouiCount.SinDate)
                            {
                                _calculationInfRepository.InsertReceCmtErr(hpId, userId, userName, _oldReceCheckErrs, _newReceCheckErrs, receInfModel, ReceErrCdConst.ExpiredEndDateHokenErrCd,
                                     ReceErrCdConst.ExpiredEndDateHokenErrMsg, "（～" + CIUtil.SDateToShowSWDate(receInfModel.PtKohi3.EndDate) + "）", KOHI3_CHAR);
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
                            if (receInfModel.PtKohi4.StartDate > 0 && receInfModel.PtKohi4.StartDate > firstSinKouiCount.SinDate)
                            {
                                _calculationInfRepository.InsertReceCmtErr(hpId, userId, userName, _oldReceCheckErrs, _newReceCheckErrs, receInfModel, ReceErrCdConst.ExpiredStartDateHokenErrCd,
                                    ReceErrCdConst.ExpiredStartDateHokenErrMsg, "（" + CIUtil.SDateToShowSWDate(receInfModel.PtKohi4.StartDate) + "～）", KOHI4_CHAR);
                            }
                        }

                        var lastSinKouiCount = _sinKouiCounts.OrderBy(p => p.SinDate).LastOrDefault(p => p.PtHokenPatterns
                            .Any(q => q.Kohi1Id == receInfModel.Kohi4Id || q.Kohi2Id == receInfModel.Kohi4Id || q.Kohi3Id == receInfModel.Kohi4Id || q.Kohi4Id == receInfModel.Kohi4Id));
                        if (lastSinKouiCount != null)
                        {
                            if (receInfModel.PtKohi4.EndDate > 0 && receInfModel.PtKohi4.EndDate < lastSinKouiCount.SinDate)
                            {
                                _calculationInfRepository.InsertReceCmtErr(hpId, userId, userName, _oldReceCheckErrs, _newReceCheckErrs, receInfModel, ReceErrCdConst.ExpiredEndDateHokenErrCd,
                                    ReceErrCdConst.ExpiredEndDateHokenErrMsg, "（～" + CIUtil.SDateToShowSWDate(receInfModel.PtKohi4.EndDate) + "）", KOHI4_CHAR);
                            }
                        }
                    }
                }
            }

            //E1003 unconfirmed
            if (_receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.UnConfirmedHokenErrCd))
            {
                if (receInfModel.HokenId > 0 && !receInfModel.IsHokenConfirmed)
                {
                    string latestConfirmedDate = string.Empty;
                    if (receInfModel.HokenChecks?.Count > 0)
                    {
                        latestConfirmedDate = CIUtil.SDateToShowSWDate(receInfModel.HokenChecks.OrderByDescending(p => p.ConfirmDate).FirstOrDefault()?.ConfirmDate ?? 0);
                    }
                    _calculationInfRepository.InsertReceCmtErr(hpId, userId, userName, _oldReceCheckErrs, _newReceCheckErrs, receInfModel, ReceErrCdConst.UnConfirmedHokenErrCd,
                            ReceErrCdConst.UnConfirmedHokenErrMsg, "（最終確認: " + latestConfirmedDate + "）", HOKEN_CHAR);
                }
                if (receInfModel.Kohi1Id > 0 && !receInfModel.IsKohi1Confirmed)
                {
                    string latestConfirmedDate = string.Empty;
                    if (receInfModel.Kohi1Checks?.Count > 0)
                    {
                        latestConfirmedDate = CIUtil.SDateToShowSWDate(receInfModel.Kohi1Checks.OrderByDescending(p => p.ConfirmDate).FirstOrDefault()?.ConfirmDate ?? 0);
                    }
                    _calculationInfRepository.InsertReceCmtErr(hpId, userId, userName, _oldReceCheckErrs, _newReceCheckErrs, receInfModel, ReceErrCdConst.UnConfirmedHokenErrCd,
                             ReceErrCdConst.UnConfirmedHokenErrMsg, "（最終確認: " + latestConfirmedDate + "）", KOHI1_CHAR);
                }
                if (receInfModel.Kohi2Id > 0 && !receInfModel.IsKohi2Confirmed)
                {
                    string latestConfirmedDate = string.Empty;
                    if (receInfModel.Kohi2Checks?.Count > 0)
                    {
                        latestConfirmedDate = CIUtil.SDateToShowSWDate(receInfModel.Kohi2Checks.OrderByDescending(p => p.ConfirmDate).FirstOrDefault()?.ConfirmDate ?? 0);
                    }
                    _calculationInfRepository.InsertReceCmtErr(hpId, userId, userName, _oldReceCheckErrs, _newReceCheckErrs, receInfModel, ReceErrCdConst.UnConfirmedHokenErrCd,
                            ReceErrCdConst.UnConfirmedHokenErrMsg, "（最終確認: " + latestConfirmedDate + "）", KOHI2_CHAR);
                }
                if (receInfModel.Kohi3Id > 0 && !receInfModel.IsKohi3Confirmed)
                {
                    string latestConfirmedDate = string.Empty;
                    if (receInfModel.Kohi3Checks?.Count > 0)
                    {
                        latestConfirmedDate = CIUtil.SDateToShowSWDate(receInfModel.Kohi3Checks.OrderByDescending(p => p.ConfirmDate).FirstOrDefault()?.ConfirmDate ?? 0);
                    }
                    _calculationInfRepository.InsertReceCmtErr(hpId, userId, userName, _oldReceCheckErrs, _newReceCheckErrs, receInfModel, ReceErrCdConst.UnConfirmedHokenErrCd,
                            ReceErrCdConst.UnConfirmedHokenErrMsg, "（最終確認: " + latestConfirmedDate + "）", KOHI3_CHAR);
                }
                if (receInfModel.Kohi4Id > 0 && !receInfModel.IsKohi4Confirmed)
                {
                    string latestConfirmedDate = string.Empty;
                    if (receInfModel.Kohi4Checks?.Count > 0)
                    {
                        latestConfirmedDate = CIUtil.SDateToShowSWDate(receInfModel.Kohi4Checks.OrderByDescending(p => p.ConfirmDate).FirstOrDefault()?.ConfirmDate ?? 0);
                    }
                    _calculationInfRepository.InsertReceCmtErr(hpId, userId, userName, _oldReceCheckErrs, _newReceCheckErrs, receInfModel, ReceErrCdConst.UnConfirmedHokenErrCd,
                            ReceErrCdConst.UnConfirmedHokenErrMsg, "（最終確認: " + latestConfirmedDate + "）", KOHI4_CHAR);
                }
            }
        }

        public void CheckByomei(int hpId, int userId, string userName, ReceInfModel receInfModel)
        {
            var ptByomeis = _calculationInfRepository.GetByomeiInThisMonth(hpId, receInfModel.SinYm, receInfModel.PtId, receInfModel.HokenId);
            if (ptByomeis.Count == 0)
            {
                //E2001 not exist byomei in month
                if (_receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.NotExistByomeiErrCd))
                {
                    var sinKouiDetail = _sinKouiCounts.FirstOrDefault(p => p.IsFirstVisit);
                    if (sinKouiDetail != null)
                    {
                        string msg2 = string.Format("（初診: {0}）", CIUtil.SDateToShowSWDate(sinKouiDetail.SinDate));
                        _calculationInfRepository.InsertReceCmtErr(hpId, userId, userName, _oldReceCheckErrs, _newReceCheckErrs, receInfModel, ReceErrCdConst.NotExistByomeiErrCd, ReceErrCdConst.NotExistByomeiErrMsg, msg2, sinDate: sinKouiDetail.SinDate);
                    }
                    else
                    {
                        _calculationInfRepository.InsertReceCmtErr(hpId, userId, userName, _oldReceCheckErrs, _newReceCheckErrs, receInfModel, ReceErrCdConst.NotExistByomeiErrCd, ReceErrCdConst.NotExistByomeiErrMsg);
                    }
                }

                //E2011 Bui Order Byomei
                if (_systemConfRepository.GetSettingValue(6003, 0, hpId) == 1 && _receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.BuiOrderByomeiErrCd))
                {
                    foreach (var sinKouiCount in _sinKouiCounts)
                    {
                        var odrInfs = _calculationInfRepository.GetOdrInfsBySinDate(hpId, receInfModel.PtId, sinKouiCount.SinDate, receInfModel.HokenId);
                        var buiOdrItemMsts = _calculationInfRepository.GetBuiOdrItemMsts(hpId);
                        var buiOdrItemByomeiMsts = _calculationInfRepository.GetBuiOdrItemByomeiMsts(hpId);
                        List<string> msgErrors = CheckBuiOrderByomei(buiOdrItemMsts, buiOdrItemByomeiMsts, odrInfs, ptByomeis);
                        if (msgErrors.Count > 0)
                        {
                            foreach (var msgError in msgErrors)
                            {
                                string itemName = odrInfs.FirstOrDefault(p => p.ItemCd == msgError)?.ItemName ?? string.Empty;
                                _calculationInfRepository.InsertReceCmtErr(hpId, userId, userName, _oldReceCheckErrs, _newReceCheckErrs, receInfModel, ReceErrCdConst.BuiOrderByomeiErrCd,
                                                                           ReceErrCdConst.BuiOrderByomeiErrMsg,
                                                                           itemName + " : " +
                                                                           CIUtil.SDateToShowSWDate(sinKouiCount.SinDate) + "）",
                                                                           msgError, sinDate: sinKouiCount.SinDate);
                            }
                        }
                    }
                }

                //E2010 Bui Order Byomei
                if (_systemConfRepository.GetSettingValue(6003, 0, hpId) == 1 && _receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.ByomeiBuiOrderByomeiChekkuErrCd))
                {
                    var odrInfModels = _calculationInfRepository.GetOdrInfModels(hpId, receInfModel.PtId, receInfModel.SinYm, receInfModel.HokenId);
                    var buiOdrMsts = _calculationInfRepository.GetBuiOdrMsts(hpId);
                    var buiOdrByomeiMsts = _calculationInfRepository.GetBuiOdrByomeiMsts(hpId);
                    List<string> errorMsgs = CheckByomeiWithBuiOdr(odrInfModels, buiOdrMsts, buiOdrByomeiMsts, ptByomeis);
                    foreach (var errorOdrInfDetail in errorOdrInfDetails)
                    {
                        foreach (var msg in errorOdrInfDetail.Errors)
                        {
                            _calculationInfRepository.InsertReceCmtErr(hpId, userId, userName, _oldReceCheckErrs, _newReceCheckErrs, receInfModel, ReceErrCdConst.ByomeiBuiOrderByomeiChekkuErrCd, ReceErrCdConst.ByomeiBuiOrderByomeiChekkuErrMsg, msg, aCd: errorOdrInfDetail.ItemName, bCd: errorOdrInfDetail.OdrKouiKbn.ToString(), sinDate: errorOdrInfDetail.SinDate);
                        }
                    }
                }
            }
            else
            {

                //check if exist continous byomei in first visit or revisit
                if (_receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.NotExistByomeiErrCd) || _receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.CheckFirstVisit2003ByomeiErrCd))
                {
                    string format = string.Empty;
                    string msg2 = string.Empty;
                    foreach (var sinKouiDetail in _sinKouiCounts)
                    {
                        //E2002 revisit
                        if (_receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.NotExistByomeiErrCd) &&
                            sinKouiDetail.IsReVisit && !ptByomeis.Any(p => p.StartDate <= sinKouiDetail.SinDate &&
                            (p.TenkiDate >= sinKouiDetail.SinDate || p.IsContinous)))
                        {
                            format = "（再診: {0}）";
                            msg2 = string.Format(format, CIUtil.SDateToShowSWDate(sinKouiDetail.SinDate));
                            _calculationInfRepository.InsertReceCmtErr(hpId, userId, userName, _oldReceCheckErrs, _newReceCheckErrs, receInfModel, ReceErrCdConst.CheckReVisitContiByomeiErrCd, ReceErrCdConst.CheckReVisitContiByomeiErrMsg, msg2, sinDate: sinKouiDetail.SinDate);
                        }
                        //first visit
                        else if (_receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.CheckFirstVisit2003ByomeiErrCd) && sinKouiDetail.IsFirstVisit)
                        {
                            //E2004
                            var checkedPtByomeis = new List<PtDiseaseModel>();
                            if (!_sinKouiCounts.Any(p => p.SinDate == sinKouiDetail.SinDate && p.ExistSameFirstVisit))
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
                                        _calculationInfRepository.InsertReceCmtErr(hpId, userId, userName, _oldReceCheckErrs, _newReceCheckErrs, receInfModel, ReceErrCdConst.CheckFirstVisit2003ByomeiErrCd, ReceErrCdConst.CheckFirstVisit2004ByomeiErrMsg,
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
                                _calculationInfRepository.InsertReceCmtErr(hpId, userId, userName, _oldReceCheckErrs, _newReceCheckErrs, receInfModel, ReceErrCdConst.CheckFirstVisit2003ByomeiErrCd, ReceErrCdConst.CheckFirstVisit2003ByomeiErrMsg, msg2, sinDate: sinKouiDetail.SinDate);
                            }
                        }
                    }
                }



                //E2005 check if has not main byomei 
                if (_receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.HasNotMainByomeiErrCd) && !ptByomeis.Any(p => p.IsMain))
                {
                    _calculationInfRepository.InsertReceCmtErr(hpId, userId, userName, _oldReceCheckErrs, _newReceCheckErrs, receInfModel, ReceErrCdConst.HasNotMainByomeiErrCd, ReceErrCdConst.HasNotMainByomeiErrMsg);
                }

                //E2006 check abandonment byomei
                if (_receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.InvalidByomeiErrCd))
                {
                    foreach (var ptByomei in ptByomeis)
                    {
                        if (!ptByomei.IsFree && ptByomei.DelDate > 0 && ptByomei.DelDate < receInfModel.FirstDateOfThisMonth &&
                            (ptByomei.TenkiDate > ptByomei.DelDate || ptByomei.IsContinous))
                        {
                            string format = "（{0}: ～{1}）";
                            string msg2 = string.Format(format, ptByomei.Byomei, CIUtil.SDateToShowSWDate(ptByomei.DelDate));
                            _calculationInfRepository.InsertReceCmtErr(hpId, userId, userName, _oldReceCheckErrs, _newReceCheckErrs, receInfModel, ReceErrCdConst.InvalidByomeiErrCd, ReceErrCdConst.InvalidByomeiErrMsg,
                                                                       msg2, ptByomei.ByomeiCd);
                        }
                    }
                }

                //E2007 check free text char count > 20
                if (_receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.FreeTextLengthByomeiErrCd))
                {
                    foreach (var ptByomei in ptByomeis)
                    {
                        if (ptByomei.IsFree && ptByomei.Byomei.Length > 20)
                        {
                            string cutByomei = CIUtil.Copy(ptByomei.Byomei, 1, 100);
                            string msg2 = string.Format("({0}: {1}/20文字)", cutByomei, ptByomei.Byomei.Length);
                            _calculationInfRepository.InsertReceCmtErr(hpId, userId, userName, _oldReceCheckErrs, _newReceCheckErrs, receInfModel, ReceErrCdConst.FreeTextLengthByomeiErrCd, ReceErrCdConst.FreeTextLengthByomeiErrMsg, msg2, cutByomei);
                        }
                    }
                }

                //E2008 check suspected byomei
                var receCheckOpt = _receCheckOpts.FirstOrDefault(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.CheckSuspectedByomeiErrCd);
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
                            _calculationInfRepository.InsertReceCmtErr(hpId, userId, userName, _oldReceCheckErrs, _newReceCheckErrs, receInfModel, ReceErrCdConst.CheckSuspectedByomeiErrCd,
                                ReceErrCdConst.CheckSuspectedByomeiErrMsg.Replace("xx", receCheckOpt.CheckOpt.AsString()), msg2, cutByomei);
                        }
                    }
                }



                bool checkDuplicateByomei = _receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.DuplicateByomeiCheckErrCd);
                bool checkDuplicateSyusyokuByomei = _receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.DuplicateSyusyokuByomeiCheckErrCd);
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
                        _calculationInfRepository.InsertReceCmtErr(hpId, userId, userName, _oldReceCheckErrs, _newReceCheckErrs, receInfModel, ReceErrCdConst.DuplicateByomeiCheckErrCd,
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
                        var odrInfs = _calculationInfRepository.GetOdrInfsBySinDate(hpId, receInfModel.PtId, sinKouiCount.SinDate, receInfModel.HokenId);

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

                                string santeiItemCd = _calculationInfRepository.GetSanteiItemCd(hpId, itemCd, sinKouiCount.SinDate);

                                List<string> tekiouByomeiCds = _calculationInfRepository.GetTekiouByomei(hpId, new List<string>() { itemCd, santeiItemCd });
                                if (tekiouByomeiCds.Count == 0) continue;

                                if (!ptByomeis.Where(p => p.StartDate <= odrInf.SinDate && (!odrInf.IsDrug || !p.Byomei.AsString().Contains(SUSPECTED_SUFFIX)))
                                             .Any(p => tekiouByomeiCds.Contains(p.ByomeiCd)))
                                {
                                    checkedItemCds.Add(odrInf.ItemCd);
                                    if (checkedItemCds.Count(p => p == odrInf.ItemCd) == 1)
                                    {
                                        _calculationInfRepository.InsertReceCmtErr(hpId, userId, userName, _oldReceCheckErrs, _newReceCheckErrs, receInfModel, ReceErrCdConst.HasNotByomeiWithOdrErrCd,
                                                                                   ReceErrCdConst.HasNotByomeiWithOdrErrMsg,
                                                                                   "（" + odrInf.ItemName + " : " +
                                                                                   CIUtil.SDateToShowSWDate(sinKouiCount.SinDate) + "）",
                                                                                   odrInf.ItemCd);
                                    }
                                }
                            }
                        }


                        //E2011 check bui order byomei
                        if (_systemConfRepository.GetSettingValue(6003, 0, hpId) == 1 && checkBuiOrderByomei)
                        {
                            var buiOdrItemMsts = _calculationInfRepository.GetBuiOdrItemMsts(hpId);
                            var buiOdrItemByomeiMsts = _calculationInfRepository.GetBuiOdrItemByomeiMsts(hpId);
                            List<string> msgErrors = CheckBuiOrderByomei(buiOdrItemMsts, buiOdrItemByomeiMsts, odrInfs, ptByomeis);
                            if (msgErrors.Count > 0)
                            {
                                foreach (var msgError in msgErrors)
                                {
                                    string itemName = odrInfs.FirstOrDefault(p => p.ItemCd == msgError)?.ItemName;
                                    _calculationInfRepository.InsertReceCmtErr(hpId, userId, userName, _oldReceCheckErrs, _newReceCheckErrs, receInfModel, ReceErrCdConst.BuiOrderByomeiErrCd,
                                                                               ReceErrCdConst.BuiOrderByomeiErrMsg,
                                                                               itemName + " : " +
                                                                               CIUtil.SDateToShowSWDate(sinKouiCount.SinDate) + "）",
                                                                               msgError, sinDate: sinKouiCount.SinDate);
                                }
                            }
                        }
                    }
                }


                if (_systemConfRepository.GetSettingValue(6003, 0, hpId) == 1 && _receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.ByomeiBuiOrderByomeiChekkuErrCd))
                {
                    var odrInfModels = _calculationInfRepository.GetOdrInfModels(hpId, receInfModel.PtId, receInfModel.SinYm, receInfModel.HokenId);
                    var buiOdrMsts = _calculationInfRepository.GetBuiOdrMsts(hpId);
                    var buiOdrByomeiMsts = _calculationInfRepository.GetBuiOdrByomeiMsts(hpId);
                    List<string> errorMsgs = CheckByomeiWithBuiOdr(odrInfModels, buiOdrMsts, buiOdrByomeiMsts, ptByomeis);
                    foreach (var errorOdrInfDetail in errorOdrInfDetails)
                    {
                        foreach (var msg in errorOdrInfDetail.Errors)
                        {
                            _calculationInfRepository.InsertReceCmtErr(hpId, userId, userName, _oldReceCheckErrs, _newReceCheckErrs, receInfModel, ReceErrCdConst.ByomeiBuiOrderByomeiChekkuErrCd, ReceErrCdConst.ByomeiBuiOrderByomeiChekkuErrMsg, msg, aCd: errorOdrInfDetail.ItemName, bCd: errorOdrInfDetail.OdrKouiKbn.ToString(), sinDate: errorOdrInfDetail.SinDate);
                        }
                    }
                }

            }
        }

        public void CheckOrder(int hpId, int userId, string userName, ReceInfModel receInfModel, List<ReceCheckOptModel> _receCheckOpts, List<SinKouiCountModel> _sinKouiCounts, List<ReceCheckErr> oldReceCheckErrs, List<ReceCheckErr> newReceCheckErrs)
        {
            bool isCheckExceedDosage = _receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.ExceededDosageOdrErrCd);
            bool isCheckDuplicateOdr = _receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.DuplicateOdrErrCd);
            bool isCheckExpiredOdr = _receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.ExpiredEndDateOdrErrCd);
            bool isCheckFirstExamFee = _receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.FirstExamFeeCheckErrCd);
            bool isCheckSanteiCount = _receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.SanteiCountCheckErrCd);
            bool isCheckTokuzaiItem = _receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.TokuzaiItemCheckErrCd);
            bool isCheckItemAge = _receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.ItemAgeCheckErrCd);
            bool isCheckComment = _receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.CommentCheckErrCd);
            bool isCheckAdditionItem = _receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.AdditionItemErrCd);

            var odrInfModels = _calculationInfRepository.GetOdrInfModels(hpId, receInfModel.PtId, receInfModel.SinYm, receInfModel.HokenId);
            List<OrdInfDetailModel> OrdInfDetailModels = new List<OrdInfDetailModel>();

            //OrderInf
            foreach (var odrInfModel in odrInfModels)
            {
                //E4001 check exceeded dosage
                if (isCheckExceedDosage)
                {
                    List<OrdInfoDetailModel> todayOdrDetails = new List<OrdInfoDetailModel>();
                    // var odrdetails = odrInfModel.OrdInfDetails.Select(p => p.OrdInfDetails).ToList();
                    foreach (var odrDetail in odrInfModel.OrdInfDetails)
                    {
                        todayOdrDetails.Add(new OrdInfoDetailModel(string.Empty, odrDetail.SinKouiKbn, odrDetail.ItemCd, odrDetail.ItemName, odrDetail.Suryo,
                                                                   odrDetail.UnitName, odrDetail.TermVal, odrDetail.SyohoKbn, odrDetail.SyohoLimitKbn,
                                                                   odrDetail.DrugKbn, odrDetail.SyohoKbn, odrDetail.IpnCd, odrDetail.Bunkatu, odrDetail.MasterSbt, odrDetail.BunkatuKoui));
                    }

                    var todayOdrInf = new OrdInfoModel(odrInfModel.OdrKouiKbn, odrInfModel.SanteiKbn, todayOdrDetails);
                    var resultOdrs = CheckOnlyDayLimitOrder(todayOdrInf, odrInfModel.PtId, odrInfModel.SinDate);
                    foreach (var odr in resultOdrs)
                    {
                        string msg2 = string.Format("（{0}: {1} [{2}日/{3}日]）", odr.ItemName, CIUtil.SDateToShowSWDate(odrInfModel.SinDate), odr.UsingDay, odr.LimitDay);
                        _calculationInfRepository.InsertReceCmtErr(hpId, userId, userName, _oldReceCheckErrs, _newReceCheckErrs, receInfModel, ReceErrCdConst.ExceededDosageOdrErrCd, ReceErrCdConst.ExceededDosageOdrErrMsg,
                                                        msg2, odr.ItemCd, sinDate: odrInfModel.SinDate);
                    }
                }
                OrdInfDetailModels.AddRange(odrInfModel.OrdInfDetails);
            }

            #region Duplicate check
            if (isCheckDuplicateOdr)
            {
                List<string> checkedOdrItemCds = new List<string>();
                foreach (var odrDetail in OrdInfDetailModels)
                {
                    int sinDate = odrDetail.SinDate;
                    int endDate = odrDetail.SinDate;
                    int syosinDate = _calculationInfRepository.GetFirstVisitWithSyosin(hpId, receInfModel.PtId, sinDate);
                    //E4002 check order with same effect
                    if (isCheckDuplicateOdr)
                    {
                        if (odrDetail.IsDrugOrInjection && !string.IsNullOrEmpty(odrDetail.YjCd))
                        {
                            var duplicatedOdr = OrdInfDetailModels.FirstOrDefault(p => CIUtil.Copy(p.YjCd, 1, 4) == CIUtil.Copy(odrDetail.YjCd, 1, 4) &&
                                                                                       p.SinDate == odrDetail.SinDate &&
                                                                                       p.RaiinNo == odrDetail.RaiinNo &&
                                                                                       p.ItemCd != odrDetail.ItemCd);
                            if (duplicatedOdr != null)
                            {
                                if (!checkedOdrItemCds.Contains(odrDetail.ItemCd) || !checkedOdrItemCds.Contains(duplicatedOdr.ItemCd))
                                {
                                    string msg2 = string.Format("（{0} : {1} [{2}]）", odrDetail.ItemName, duplicatedOdr.ItemName, CIUtil.SDateToShowSWDate(odrDetail.SinDate));
                                    _calculationInfRepository.InsertReceCmtErr(hpId, userId, userName, _oldReceCheckErrs, _newReceCheckErrs, receInfModel, ReceErrCdConst.DuplicateOdrErrCd, ReceErrCdConst.DuplicateOdrErrMsg,
                                                                    msg2, odrDetail.ItemCd, sinDate: odrDetail.SinDate);
                                }
                                checkedOdrItemCds.Add(odrDetail.ItemCd);
                                checkedOdrItemCds.Add(duplicatedOdr.ItemCd);
                            }
                        }
                    }
                }
            }

            #endregion

            #region Expired check
            //E3001,E3002 check expired end date and start date
            if (isCheckExpiredOdr)
            {
                List<string> checkedItemCds = new List<string>();
                foreach (var sinKouiCount in _sinKouiCounts)
                {
                    foreach (var sinKouiDetailModel in sinKouiCount.SinKouiDetailModels)
                    {
                        string itemCd = sinKouiDetailModel.ItemCd;
                        if (!string.IsNullOrEmpty(itemCd) && sinKouiDetailModel.TenMst != null && !checkedItemCds.Contains(itemCd))
                        {
                            var lastTenMst = _calculationInfRepository.FindLastTenMst(hpId, itemCd);
                            if (lastTenMst != null && sinKouiCount.SinDate > lastTenMst.EndDate)
                            {
                                string msg2 = string.Format("（{0} {1}: ～{2}）", sinKouiDetailModel.ItemName, CIUtil.SDateToShowSWDate(sinKouiCount.SinDate), CIUtil.SDateToShowSWDate(lastTenMst.EndDate));
                                _calculationInfRepository.InsertReceCmtErr(hpId, userId, userName, _oldReceCheckErrs, _newReceCheckErrs, receInfModel, ReceErrCdConst.ExpiredEndDateOdrErrCd, ReceErrCdConst.ExpiredEndDateOdrErrMsg, msg2, itemCd, sinDate: sinKouiCount.SinDate);
                            }

                            var firstTenMst = _calculationInfRepository.FindFirstTenMst(hpId, itemCd);
                            if (firstTenMst != null && sinKouiCount.SinDate < firstTenMst.StartDate)
                            {
                                string msg2 = string.Format("（{0} {1}: {2}～）", sinKouiDetailModel.ItemName, CIUtil.SDateToShowSWDate(sinKouiCount.SinDate), CIUtil.SDateToShowSWDate(firstTenMst.StartDate));
                                _calculationInfRepository.InsertReceCmtErr(hpId, userId, userName, _oldReceCheckErrs, _newReceCheckErrs, receInfModel, ReceErrCdConst.ExpiredStartDateOdrErrCd, ReceErrCdConst.ExpiredStartDateOdrErrMsg, msg2, itemCd, sinDate: sinKouiCount.SinDate);
                            }

                            checkedItemCds.Add(itemCd);
                        }
                    }
                }
            }

            #endregion

            #region Santei count check

            //E3004 check santei count as file checkingViewModel function CalculationCountCheck
            if (isCheckSanteiCount)
            {
                List<string> checkedItemCds = new List<string>();
                foreach (var sinKouiCount in _sinKouiCounts)
                {
                    int sinDate = sinKouiCount.SinDate;
                    long raiinNo = sinKouiCount.RaiinNo;
                    var hokenInf = sinKouiCount.PtHokenPatterns.FirstOrDefault(p => p.HokenId == receInfModel.HokenId);
                    foreach (var sinKouiDetailModel in sinKouiCount.SinKouiDetailModels)
                    {
                        string itemCd = sinKouiDetailModel.ItemCd;
                        if (!string.IsNullOrEmpty(itemCd) && sinKouiDetailModel.TenMst != null)
                        {
                            #region Sub function
                            int WeeksBefore(int baseDate, int term)
                            {
                                return CIUtil.WeeksBefore(baseDate, term);
                            }

                            int MonthsBefore(int baseDate, int term)
                            {
                                return CIUtil.MonthsBefore(baseDate, term);
                            }

                            int YearsBefore(int baseDate, int term)
                            {
                                return CIUtil.YearsBefore(baseDate, term);
                            }

                            int DaysBefore(int baseDate, int term)
                            {
                                return CIUtil.DaysBefore(baseDate, term);
                            }

                            int MonthsAfter(int baseDate, int term)
                            {
                                return CIUtil.MonthsAfter(baseDate, term);
                            }

                            int GetHokenKbn(int receHokenKbn)
                            {
                                int hokenKbn = 0;

                                if (new int[] { 0 }.Contains(receHokenKbn))
                                {
                                    hokenKbn = 4;
                                }
                                else if (new int[] { 1, 2 }.Contains(receHokenKbn))
                                {
                                    hokenKbn = 0;
                                }
                                else if (new int[] { 11, 12 }.Contains(receHokenKbn))
                                {
                                    hokenKbn = 1;
                                }
                                else if (new int[] { 13 }.Contains(receHokenKbn))
                                {
                                    hokenKbn = 2;
                                }
                                else if (new int[] { 14 }.Contains(receHokenKbn))
                                {
                                    hokenKbn = 3;
                                }

                                return hokenKbn;
                            }
                            /// <summary>
                            /// チェック用保険区分を返す
                            /// 健保、労災、自賠の場合、オプションにより、同一扱いにするか別扱いにするか決定
                            /// 自費の場合、健保と自費を対象にする
                            /// </summary>
                            /// <param name="hokenKbn">
                            /// 0-健保、1-労災、2-アフターケア、3-自賠、4-自費
                            /// </param>
                            /// <returns></returns>
                            List<int> GetCheckHokenKbns(int receHokenKbn)
                            {
                                /// <summary>
                                /// 保険区分
                                ///     0:自費
                                ///     1:社保          
                                ///     2:国保          
                                ///     11:労災(短期給付)          
                                ///     12:労災(傷病年金)          
                                ///     13:アフターケア          
                                ///     14:自賠責          
                                /// </summary>

                                List<int> results = new List<int>();

                                int hokenKbn = GetHokenKbn(receHokenKbn);


                                if (_systemConfRepository.GetSettingValue(3013, 0, hpId) == 0)
                                {
                                    // 同一に考える
                                    if (hokenKbn <= 3)
                                    {
                                        results.AddRange(new List<int> { 0, 1, 2, 3 });
                                    }
                                    else
                                    {
                                        results.Add(hokenKbn);
                                    }
                                }
                                else if (_systemConfRepository.GetSettingValue(3013, 0, hpId) == 1)
                                {
                                    // すべて同一に考える
                                    results.AddRange(new List<int> { 0, 1, 2, 3, 4 });
                                }
                                else
                                {
                                    // 別に考える
                                    results.Add(hokenKbn);
                                }

                                if (hokenKbn == 4)
                                {
                                    results.Add(0);
                                }

                                return results;
                            }

                            List<int> GetCheckSanteiKbns(int receHokenKbn)
                            {
                                List<int> results = new List<int> { 0 };
                                int hokenKbn = GetHokenKbn(receHokenKbn);

                                if (_systemConfRepository.GetSettingValue(3013, 0, hpId) == 0)
                                {
                                    // 同一に考える
                                    if (hokenKbn == 4)
                                    {
                                        //results.Add(2);
                                    }
                                }
                                else if (_systemConfRepository.GetSettingValue(3013, 0, hpId) == 1)
                                {
                                    // すべて同一に考える
                                    results.Add(2);
                                }
                                else
                                {
                                    // 別に考える
                                }

                                return results;
                            }
                            #endregion

                            List<DensiSanteiKaisuModel> densiSanteiKaisuModels = _masterFinder.FindDensiSanteiKaisuList(sinDate, itemCd);
                            if (hokenInf != null && (hokenInf.HokenKbn == 11 || hokenInf.HokenKbn == 12 || hokenInf.HokenKbn == 13
                                || (hokenInf.HokenKbn == 14 && SystemConfig.Instance.JibaiJunkyo == 1)))
                            {
                                densiSanteiKaisuModels = densiSanteiKaisuModels.FindAll(p => p.TargetKbn == 2 || p.TargetKbn == 0);
                            }
                            else
                            {
                                densiSanteiKaisuModels = densiSanteiKaisuModels.FindAll(p => p.TargetKbn == 1 || p.TargetKbn == 0);
                            }
                            foreach (var densiSanteiKaisu in densiSanteiKaisuModels)
                            {
                                string sTerm = string.Empty;
                                int startDate = 0;
                                // チェック終了日
                                int endDate = sinDate;

                                List<int> checkHokenKbnTmp = new List<int>();
                                checkHokenKbnTmp.AddRange(GetCheckHokenKbns(receInfModel.HokenKbn));

                                if (densiSanteiKaisu.TargetKbn == 1)
                                {
                                    // 健保のみ対象の場合はすべて対象
                                }
                                else if (densiSanteiKaisu.TargetKbn == 2)
                                {
                                    // 労災のみ対象の場合、健保は抜く
                                    checkHokenKbnTmp.RemoveAll(p => new int[] { 0 }.Contains(p));
                                }

                                List<int> checkSanteiKbnTmp = new List<int>();
                                checkSanteiKbnTmp.AddRange(GetCheckSanteiKbns(receInfModel.HokenKbn));

                                switch (densiSanteiKaisu.UnitCd)
                                {
                                    case 53:    //患者あたり
                                        sTerm = "患者あたり";
                                        break;
                                    case 121:   //1日
                                        startDate = sinDate;
                                        sTerm = "日";
                                        break;
                                    case 131:   //1月
                                        startDate = sinDate / 100 * 100 + 1;
                                        sTerm = "月";
                                        break;
                                    case 138:   //1週
                                        startDate = WeeksBefore(sinDate, 1);
                                        sTerm = "週";
                                        break;
                                    case 141:   //一連
                                        startDate = -1;
                                        sTerm = "一連";
                                        break;
                                    case 142:   //2週
                                        startDate = WeeksBefore(sinDate, 2);
                                        sTerm = "2週";
                                        break;
                                    case 143:   //2月
                                        startDate = MonthsBefore(sinDate, 1);
                                        sTerm = "2月";
                                        break;
                                    case 144:   //3月
                                        startDate = MonthsBefore(sinDate, 2);
                                        sTerm = "3月";
                                        break;
                                    case 145:   //4月
                                        startDate = MonthsBefore(sinDate, 3);
                                        sTerm = "4月";
                                        break;
                                    case 146:   //6月
                                        startDate = MonthsBefore(sinDate, 5);
                                        sTerm = "6月";
                                        break;
                                    case 147:   //12月
                                        startDate = MonthsBefore(sinDate, 11);
                                        sTerm = "12月";
                                        break;
                                    case 148:   //5年
                                        startDate = YearsBefore(sinDate, 5);
                                        sTerm = "5年";
                                        break;
                                    case 999:   //カスタム
                                        if (densiSanteiKaisu.TermSbt == 2)
                                        {
                                            //日
                                            startDate = DaysBefore(sinDate, densiSanteiKaisu.TermCount);
                                            if (densiSanteiKaisu.TermCount == 1)
                                            {
                                                sTerm = "日";
                                            }
                                            else
                                            {
                                                sTerm = densiSanteiKaisu.TermCount + "日";
                                            }
                                        }
                                        else if (densiSanteiKaisu.TermSbt == 3)
                                        {
                                            //週
                                            startDate = WeeksBefore(sinDate, densiSanteiKaisu.TermCount);
                                            if (densiSanteiKaisu.TermCount == 1)
                                            {
                                                sTerm = "週";
                                            }
                                            else
                                            {
                                                sTerm = densiSanteiKaisu.TermCount + "週";
                                            }
                                        }
                                        else if (densiSanteiKaisu.TermSbt == 4)
                                        {
                                            //月
                                            startDate = MonthsBefore(sinDate, densiSanteiKaisu.TermCount);
                                            if (densiSanteiKaisu.TermCount == 1)
                                            {
                                                sTerm = "月";
                                            }
                                            else
                                            {
                                                sTerm = densiSanteiKaisu.TermCount + "月";
                                            }
                                        }
                                        else if (densiSanteiKaisu.TermSbt == 5)
                                        {
                                            //年間
                                            startDate = (sinDate / 10000 - (densiSanteiKaisu.TermCount - 1)) * 10000 + 101;
                                            if (densiSanteiKaisu.TermCount == 1)
                                            {
                                                sTerm = "年間";
                                            }
                                            else
                                            {
                                                sTerm = densiSanteiKaisu.TermCount + "年間";
                                            }
                                        }
                                        break;
                                    default:
                                        startDate = -1;
                                        break;
                                }

                                List<string> itemCds = new List<string>();

                                List<ItemGrpMstModel> itemGrpMsts = new List<ItemGrpMstModel>();

                                if (densiSanteiKaisu.ItemGrpCd > 0)
                                {
                                    // 項目グループの設定がある場合
                                    itemGrpMsts = _recalculationFinder.FindItemGrpMst(sinDate, 1, densiSanteiKaisu.ItemGrpCd);
                                }

                                if (itemGrpMsts != null && itemGrpMsts.Any())
                                {
                                    // 項目グループの設定がある場合
                                    itemCds.AddRange(itemGrpMsts.Select(x => x.ItemCd));
                                }
                                else
                                {
                                    itemCds.Add(itemCd);
                                }

                                double santeiCount = 0;
                                if (startDate >= 0)
                                {
                                    santeiCount = _masterFinder.SanteiCount(receInfModel.PtId, startDate, sinDate,
                                                                   sinDate, 0, itemCds, checkSanteiKbnTmp, checkHokenKbnTmp);
                                }

                                if (santeiCount > densiSanteiKaisu.MaxCount)
                                {
                                    string msg2 = string.Format("({0}: {1}回 [{2}回/{3}])", sinKouiDetailModel.ItemName, santeiCount, densiSanteiKaisu.MaxCount, sTerm);
                                    _commandHandler._calculationInfRepository.InsertReceCmtErr(receInfModel, ReceErrCdConst.SanteiCountCheckErrCd, ReceErrCdConst.SanteiCountCheckErrMsg, msg2, itemCd);
                                }
                            }
                            checkedItemCds.Add(itemCd);
                        }
                    }
                }
            }

            #endregion


            //E3003 check first exam fee
            if (isCheckFirstExamFee)
            {
                double suryoSum = 0;
                string msg2 = string.Empty;
                foreach (var sinKouiCount in __sinKouiCounts)
                {
                    if (sinKouiCount.IsFirstVisit)
                    {
                        //msg2 max length = 100
                        string formatSinDate = CIUtil.SDateToShowSWDate(sinKouiCount.SinDate);
                        if (!msg2.Contains(formatSinDate) && msg2.Length + formatSinDate.Length + 2 <= 100)
                        {
                            if (!string.IsNullOrEmpty(msg2))
                            {
                                msg2 += ", ";
                            }
                            msg2 += formatSinDate;
                        }
                        suryoSum += sinKouiCount.SinKouiDetailModels.Where(p => ReceErrCdConst.IsFirstVisitCd.Contains(p.ItemCd)).Sum(p => p.Suryo);
                    }
                }
                if (suryoSum > 1)
                {
                    _commandHandler._calculationInfRepository.InsertReceCmtErr(receInfModel, ReceErrCdConst.FirstExamFeeCheckErrCd, ReceErrCdConst.FirstExamFeeCheckErrMsg, msg2);
                }
            }

            //E3005 check tokuzai item
            if (isCheckTokuzaiItem)
            {
                foreach (var sinKouiCount in _sinKouiCounts)
                {
                    if (sinKouiCount.SinKouiDetailModels.Any(p => p.ItemCd == ReceErrCdConst.TokuzaiItemCd))
                    {
                        _calculationInfRepository.InsertReceCmtErr(hpId, userId, userName, _oldReceCheckErrs, _newReceCheckErrs, receInfModel, ReceErrCdConst.TokuzaiItemCheckErrCd, ReceErrCdConst.TokuzaiItemCheckErrMsg,
                                                        "（2017(H29)/04/01～使用不可）", ReceErrCdConst.TokuzaiItemCd, sinDate: sinKouiCount.SinDate);
                        continue;
                    }
                }
            }


            try
            {
                //E3007 check patient age to use order
                if (isCheckItemAge)
                {
                    List<string> checkedItemCds = new List<string>();
                    int iBirthDay = receInfModel.Birthday;
                    foreach (var sinKouiCount in __sinKouiCounts)
                    {
                        foreach (var sinKouiDetailModel in sinKouiCount.SinKouiDetailModels)
                        {
                            string itemCd = sinKouiDetailModel.ItemCd;
                            string maxAge = sinKouiDetailModel.MaxAge;
                            string minAge = sinKouiDetailModel.MinAge;
                            if (!string.IsNullOrEmpty(sinKouiDetailModel.ItemCd) && sinKouiDetailModel.TenMst != null && !checkedItemCds.Contains(itemCd))
                            {
                                #region sub function
                                int iYear = 0;
                                int iMonth = 0;
                                int iDay = 0;
                                CIUtil.SDateToDecodeAge(iBirthDay, sinKouiCount.SinDate, ref iYear, ref iMonth, ref iDay);

                                // Total day from birthday to sindate
                                int iDays = 0;
                                if (iBirthDay < sinKouiCount.SinDate)
                                {
                                    iDays = CIUtil.DaysBetween(CIUtil.StrToDate(CIUtil.SDateToShowSDate(iBirthDay)), CIUtil.StrToDate(CIUtil.SDateToShowSDate(sinKouiCount.SinDate)));
                                }

                                // tenMstAgeCheck = TenMst.MinAge or TenMst.MaxAge
                                bool _CheckInBirthMonth(int tenMstAgeCheck, int sinDate)
                                {
                                    return (iYear > tenMstAgeCheck) ||
                                           ((iYear == tenMstAgeCheck) && ((iBirthDay % 10000 / 100) < (sinDate % 10000 / 100)));
                                }

                                // tenMstAgeCheck = TenMst.MinAge or TenMst.MaxAge
                                bool _CheckAge(string tenMstAgeCheck, int sinDate)
                                {
                                    bool subResult = false;

                                    if (tenMstAgeCheck == "AA")
                                    {
                                        // 生後２８日
                                        subResult = (iDays >= 28);
                                    }
                                    else if (tenMstAgeCheck == "B3")
                                    {
                                        //３歳に達した日の翌月の１日
                                        subResult = _CheckInBirthMonth(3, sinDate);
                                    }
                                    else if (tenMstAgeCheck == "B6")
                                    {
                                        //６歳に達した日の翌月の１日
                                        subResult = _CheckInBirthMonth(6, sinDate);
                                    }
                                    else if (tenMstAgeCheck == "BF")
                                    {
                                        //１５歳に達した日の翌月の１日（現状入院項目のみ）
                                        subResult = _CheckInBirthMonth(15, sinDate);
                                    }
                                    else if (tenMstAgeCheck == "BK")
                                    {
                                        //２０歳に達した日の翌月の１日（現状入院項目のみ）
                                        subResult = _CheckInBirthMonth(20, sinDate);
                                    }
                                    else if (tenMstAgeCheck == "AE")
                                    {
                                        //生後９０日
                                        subResult = (iDays >= 90);
                                    }
                                    else if (tenMstAgeCheck == "MG")
                                    {
                                        //未就学
                                        subResult = CIUtil.IsStudent(iBirthDay, sinDate);
                                    }
                                    else
                                    {
                                        subResult = iYear >= CIUtil.StrToIntDef(tenMstAgeCheck, 0);
                                    }
                                    return subResult;
                                }

                                // tenMstAgeCheck = TenMst.MinAge or TenMst.MaxAge
                                string FormatDisplayMessage(string tenMstAgeCheck)
                                {
                                    string formatedCheckKbn = string.Empty;

                                    if (tenMstAgeCheck == "AA")
                                    {
                                        // 生後２８日
                                        formatedCheckKbn = "生後２８日";
                                    }
                                    else if (tenMstAgeCheck == "B3")
                                    {
                                        //３歳に達した日の翌月の１日
                                        formatedCheckKbn = "３歳に達した日の翌月の１日";
                                    }
                                    else if (tenMstAgeCheck == "B6")
                                    {
                                        //６歳に達した日の翌月の１日
                                        formatedCheckKbn = "６歳に達した日の翌月の１日";
                                    }
                                    else if (tenMstAgeCheck == "BF")
                                    {
                                        //１５歳に達した日の翌月の１日（現状入院項目のみ）
                                        formatedCheckKbn = "１５歳に達した日の翌月の１日";
                                    }
                                    else if (tenMstAgeCheck == "BK")
                                    {
                                        //２０歳に達した日の翌月の１日（現状入院項目のみ）
                                        formatedCheckKbn = "２０歳に達した日の翌月の１日";
                                    }
                                    else if (tenMstAgeCheck == "AE")
                                    {
                                        //生後９０日
                                        formatedCheckKbn = "生後９０日";
                                    }
                                    else if (tenMstAgeCheck == "MG")
                                    {
                                        //未就学
                                        formatedCheckKbn = "未就学";
                                    }
                                    else
                                    {
                                        formatedCheckKbn = CIUtil.StrToIntDef(tenMstAgeCheck, 0) + "歳";
                                    }
                                    return formatedCheckKbn;
                                }
                                #endregion
                                bool needCheckMaxAage = !string.IsNullOrEmpty(maxAge) && maxAge != "00" && maxAge != "0";
                                bool needCheckMinAge = !string.IsNullOrEmpty(minAge) && minAge != "00" && minAge != "0";
                                string msg2 = string.Empty;
                                if (needCheckMaxAage
                                    && needCheckMinAge
                                    && (_CheckAge(maxAge, sinKouiCount.SinDate) || !_CheckAge(minAge, sinKouiCount.SinDate)))
                                {
                                    msg2 = string.Format("（{0}: {1} [{2}～{3}]）",
                                        sinKouiDetailModel.ItemName,
                                        CIUtil.SDateToShowSWDate(sinKouiCount.SinDate),
                                        FormatDisplayMessage(minAge),
                                        FormatDisplayMessage(maxAge));
                                }
                                else if (needCheckMaxAage && _CheckAge(maxAge, sinKouiCount.SinDate))
                                {
                                    msg2 = string.Format("（{0}: {1} [～{2}]）",
                                        sinKouiDetailModel.ItemName,
                                        CIUtil.SDateToShowSWDate(sinKouiCount.SinDate),
                                        FormatDisplayMessage(maxAge));
                                }
                                else if (needCheckMinAge && !_CheckAge(minAge, sinKouiCount.SinDate))
                                {
                                    msg2 = string.Format("（{0}: {1} [{2}～]）",
                                        sinKouiDetailModel.ItemName,
                                        CIUtil.SDateToShowSWDate(sinKouiCount.SinDate),
                                        FormatDisplayMessage(minAge));
                                }
                                if (!string.IsNullOrEmpty(msg2))
                                {
                                    _calculationInfRepository.InsertReceCmtErr(hpId, userId, userName, _oldReceCheckErrs, _newReceCheckErrs, receInfModel, ReceErrCdConst.ItemAgeCheckErrCd, ReceErrCdConst.ItemAgeCheckErrMsg, msg2, itemCd, sinDate: sinKouiCount.SinDate);
                                }

                                checkedItemCds.Add(itemCd);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteLogError(_modulName, this, nameof(CheckOrder), ex, $"CheckErrCd: {ReceErrCdConst.ItemAgeCheckErrCd}, PtId: {receInfModel.PtId}");
            }

            try
            {
                //E3008 check comment into order
                if (isCheckComment)
                {
                    List<SinKouiModel> listSinKoui = _recalculationFinder.GetListSinKoui(receInfModel.PtId, receInfModel.SinYm, receInfModel.HokenId);
                    List<string> listReceCmtItemCode = _recalculationFinder.GetListReceCmtItemCode(receInfModel.PtId, receInfModel.SinYm, receInfModel.HokenId);

                    List<SinKouiDetail> listItemCdOfMonth = new List<SinKouiDetail>();
                    listSinKoui.ForEach((sinKoui) =>
                    {
                        listItemCdOfMonth.AddRange(sinKoui.SinKouiDetailModels.Select(s => s.SinKouiDetail).ToList());
                    });

                    listSinKoui.ForEach((sinKoui) =>
                    {
                        if (sinKoui.ExistItemWithCommentSelect)
                        {
                            var listItemWithCmtSelect = sinKoui.SinKouiDetailModels.Where(s => s.ListCmtSelect != null && s.ListCmtSelect.Count > 0).ToList();

                            listItemWithCmtSelect.ForEach((sinKouiDetail) =>
                            {
                                List<string> listItemCd = sinKoui.SinKouiDetailModels.Select(s => s.ItemCd).ToList();
                                sinKouiDetail.ListCmtSelect.ForEach((cmtSelect) =>
                                {
                                    cmtSelect.ListGroupComment.ForEach((groupComment) =>
                                    {
                                        List<RecedenCmtSelectModel> filteredCmtSelect = groupComment.ListRecedenCmtSelect.Where(r => r.CondKbn == 1).ToList();
                                        if (filteredCmtSelect.Count > 0)
                                        {
                                            bool existCmtSelect = false;
                                            foreach (var recedenCmtSelect in filteredCmtSelect)
                                            {
                                                if (recedenCmtSelect.IsSatsueiBui)
                                                {
                                                    // If recedenCmtSelect is 撮影部位 type => have to check in the same RP
                                                    if (listItemCd.Contains(recedenCmtSelect.CmtCd))
                                                    {
                                                        existCmtSelect = true;
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    // If recedenCmtSelect isn't 撮影部位 type => have to check in the same month
                                                    // If exist recedenCmtSelect in ReceCmt => it's Ok
                                                    bool isExistInReceCmt = listReceCmtItemCode.Contains(recedenCmtSelect.CmtCd);
                                                    if (isExistInReceCmt || listItemCdOfMonth.Any(x => x.ItemCd == recedenCmtSelect.CmtCd))
                                                    {
                                                        existCmtSelect = true;
                                                        break;
                                                    }
                                                }

                                                // Fix bug 4858
                                                if (recedenCmtSelect.CmtSbt == 3)
                                                {
                                                    //Fix comment 4818
                                                    if (listItemCdOfMonth.Any(x => x.ItemCd == ItemCdConst.CommentJissiRekkyoItemNameDummy && x.CmtOpt == sinKouiDetail.ItemCd))
                                                    {
                                                        existCmtSelect = true;
                                                        break;
                                                    }
                                                }
                                            }

                                            if (!existCmtSelect)
                                            {
                                                string itemCd = sinKouiDetail.ItemCd;
                                                string itemName = sinKouiDetail.ItemName;
                                                string cmtCd = filteredCmtSelect.First().CmtCd;

                                                string comment = filteredCmtSelect.First().CommentName;
                                                if (filteredCmtSelect.Count > 1)
                                                {
                                                    comment += "...など";
                                                }

                                                string message = string.Format("（{0}: {1}）", itemName, comment);
                                                _commandHandler._calculationInfRepository.InsertReceCmtErr(receInfModel, ReceErrCdConst.CommentCheckErrCd, ReceErrCdConst.CommentCheckErrMsg, message, itemCd, cmtCd, 0);
                                            }
                                        }
                                    });
                                });
                            });
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Log.WriteLogError(_modulName, this, nameof(CheckOrder), ex, $"CheckErrCd: {ReceErrCdConst.CommentCheckErrCd}, PtId: {receInfModel.PtId}");
            }

            try
            {
                if (isCheckAdditionItem)
                {
                    var addtionItems = _recalculationFinder.GetAddtionItems(receInfModel.PtId, receInfModel.SinYm, receInfModel.HokenId);
                    if (addtionItems.Count > 0)
                    {
                        foreach (var item in addtionItems)
                        {
                            _commandHandler._calculationInfRepository.InsertReceCmtErr(receInfModel, ReceErrCdConst.AdditionItemErrCd, CIUtil.Copy(item.Text, 1, 100), "（" + CIUtil.SDateToShowSWDate(item.SinDate) + "）",
                                item.ItemCd + "," + item.DelItemCd, item.TermCnt + "," + item.TermSbt + "," + item.IsWarning, item.SinDate);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteLogError(_modulName, this, nameof(CheckOrder), ex, $"CheckErrCd: {ReceErrCdConst.AdditionItemErrCd}, PtId: {receInfModel.PtId}");
            }

            Log.WriteLogEnd(ModuleNameConst.EmrCommonView, this, nameof(CheckOrder), ICDebugConf.logLevel);
        }

        public List<string> CheckBuiOrderByomei(List<BuiOdrItemMstModel> buiOdrItemMsts, List<BuiOdrItemByomeiMstModel> buiOdrItemByomeiMsts, List<OrdInfDetailModel> todayOrderInfModels, List<PtDiseaseModel> ptByomeiModels)
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

        public List<string> CheckByomeiWithBuiOdr(List<OrdInfModel> odrInfModels, List<BuiOdrMstModel> buiOdrMsts, List<BuiOdrByomeiMstModel> buiOdrByomeiMsts, List<PtDiseaseModel> ptByomeis)
        {
            bool IsSpecialComment(OrdInfDetailModel detail)
            {
                return detail.SinKouiKbn == 99 && !string.IsNullOrEmpty(detail.CmtOpt);
            }

            errorOdrInfDetails = new List<BuiErrorModel>();
            List<string> errorMsgs = new List<string>();
            foreach (var odrInf in odrInfModels)
            {
                var OrdInfDetailModels = odrInf.OrdInfDetails.Where(x => string.IsNullOrEmpty(x.ItemCd) || x.ItemCd.Length == 4 || x.SinKouiKbn == 99);
                foreach (var detail in OrdInfDetailModels)
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

        public bool CheckDuplicateByomei(bool checkDuplicateByomei, bool checkDuplicateSyusyokuByomei, PtDiseaseModel currentPtByomeiModel, PtDiseaseModel comparedPtByomeiModel, int recehokenId)
        {
            if (!checkDuplicateByomei)
            {
                return false;
            }
            if (currentPtByomeiModel.IsFree || comparedPtByomeiModel.IsFree ||
                (currentPtByomeiModel.HokenId != 0 && currentPtByomeiModel.HokenId != recehokenId) ||
               (comparedPtByomeiModel.HokenId != 0 && comparedPtByomeiModel.HokenId != recehokenId) ||
               currentPtByomeiModel.ByomeiCd != comparedPtByomeiModel.ByomeiCd)
            {
                return false;
            }
            int currentTenkiDate = currentPtByomeiModel.TenkiDate;
            int comparedTenkiDate = comparedPtByomeiModel.TenkiDate;
            if (currentTenkiDate == 0)
            {
                currentTenkiDate = 99999999;
            }
            if (comparedTenkiDate == 0)
            {
                comparedTenkiDate = 99999999;
            }
            if (currentTenkiDate < comparedPtByomeiModel.StartDate || comparedTenkiDate < currentPtByomeiModel.StartDate)
            {
                return false;
            }
            if (checkDuplicateSyusyokuByomei)
            {
                List<string> syusyokuCds = currentPtByomeiModel.GetAllSyusyokuCds();
                List<string> compareSyusyokuCds = comparedPtByomeiModel.GetAllSyusyokuCds();
                if (syusyokuCds.Count != compareSyusyokuCds.Count)
                {
                    return false;
                }
                for (int i = 0; i < syusyokuCds.Count; i++)
                {
                    if (syusyokuCds[i] != compareSyusyokuCds[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private List<DayLimitResultModel> CheckOnlyDayLimitOrder(OrdInfoModel todayOdrInfModel, long ptId, int sinDate)
        {
            var result = _commonMedicalCheck.CheckOnlyDayLimit(todayOdrInfModel);

            RealtimeChecker<TodayOdrInfModel, TodayOdrInfDetailModel> realtimeChecker = new RealtimeChecker<TodayOdrInfModel, TodayOdrInfDetailModel>();
            realtimeChecker.InjectProperties(Session.HospitalID, ptId, sinDate);
            realtimeChecker.InjectFinder(_realtimeCheckerFinder, _masterFinder);
            return realtimeChecker.CheckOnlyDayLimit(todayOdrInfModel);
        }

    }
}
