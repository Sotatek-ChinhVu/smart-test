using CommonChecker.DB;
using CommonChecker.Models.OrdInf;
using CommonChecker.Models.OrdInfDetailModel;
using CommonCheckers.OrderRealtimeChecker.Models;
using CommonCheckers.OrderRealtimeChecker.Services;
using Domain.CalculationInf;
using Domain.Models.Diseases;
using Domain.Models.Medical;
using Domain.Models.MstItem;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Domain.Models.Receipt;
using Domain.Models.Receipt.Recalculation;
using Domain.Models.SystemConf;
using Domain.Models.TodayOdr;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Helper.Messaging;
using Helper.Messaging.Data;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using Interactor.CalculateService;
using Interactor.CommonChecker.CommonMedicalCheck;
using UseCase.MedicalExamination.Calculate;
using UseCase.ReceiptCheck.Recalculation;
using Request = UseCase.Receipt.Recalculation;
using System.Linq;
using System.Text;

namespace Interactor.ReceiptCheck
{
    public class ReceiptCheckRecalculationInteractor : IReceiptCheckRecalculationInputPort
    {
        private const string HOKEN_CHAR = "0";
        private const string KOHI1_CHAR = "1";
        private const string KOHI2_CHAR = "2";
        private const string KOHI3_CHAR = "3";
        private const string KOHI4_CHAR = "4";
        private const string SUSPECTED_SUFFIX = "の疑い";
        private const string LEFT = "左";
        private const string RIGHT = "右";
        private const string BOTH = "両";
        private const string LEFT_RIGHT = "左右";
        private const string RIGHT_LEFT = "右左";

        private readonly ICalculateService _calculateService;
        private readonly ICalculationInfRepository _calculationInfRepository;
        private readonly ISystemConfRepository _systemConfRepository;
        private readonly ICommonMedicalCheck _commonMedicalCheck;
        private readonly IRealtimeOrderErrorFinder _realtimeOrderErrorFinder;
        private readonly ITenantProvider _tenantProvider;
        private readonly IReceiptRepository _receiptRepository;
        private readonly ILoggingHandler _loggingHandler;
        private IMessenger? _messenger;

        private int seikyuYm;
        private List<ReceInfModel> _receInfModels = new();
        private List<ReceCheckOptModel> _receCheckOpts = new();
        private List<SinKouiCountModel> _sinKouiCounts = new();
        private readonly List<ReceCheckErrModel> _newReceCheckErrs = new();
        private List<ReceCheckErrModel> _oldReceCheckErrs = new();
        private List<BuiErrorModel> _errorOdrInfDetails = new();
        public StringBuilder ErrorText { get; set; } = new();

        public ReceiptCheckRecalculationInteractor(
            ICalculateService calculateService,
            ICalculationInfRepository calculationInfRepository,
            ISystemConfRepository systemConfRepository,
            ICommonMedicalCheck commonMedicalCheck,
            IRealtimeOrderErrorFinder realtimeOrderErrorFinder,
            ITenantProvider tenantProvider,
            IReceiptRepository receiptRepository)
        {
            _calculateService = calculateService;
            _calculationInfRepository = calculationInfRepository;
            _systemConfRepository = systemConfRepository;
            _commonMedicalCheck = commonMedicalCheck;
            _realtimeOrderErrorFinder = realtimeOrderErrorFinder;
            _tenantProvider = tenantProvider;
            _receiptRepository = receiptRepository;
            _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
        }

        public ReceiptCheckRecalculationOutputData Handle(ReceiptCheckRecalculationInputData inputData)
        {
            _messenger = inputData.Messenger;

            string errorText = string.Empty;
            try
            {
                if (DateTime.Now.Day <= 10)
                {
                    if (DateTime.Now.Month > 1)
                    {
                        seikyuYm = DateTime.Now.Year * 100 + DateTime.Now.Month - 1;
                    }
                    else
                    {
                        seikyuYm = (DateTime.Now.Year - 1) * 100 + 12;
                    }
                }
                else
                {
                    seikyuYm = DateTime.Now.Year * 100 + DateTime.Now.Month;
                }

                SendMessenger(new RecalculationStatus(false, CalculateStatusConstant.RecalculationCheckBox, 0, 0, "再計算中・・・", "NotConnectSocket"));
                _calculateService.RunCalculateMonth(
                    new Request.CalculateMonthRequest()
                    {
                        HpId = inputData.HpId,
                        SeikyuYm = inputData.SeikyuYm,
                        PtIds = inputData.PtIds,
                        PreFix = inputData.UserId.ToString(),
                    });

                SendMessenger(new RecalculationStatus(false, CalculateStatusConstant.ReceiptAggregationCheckBox, 0, 0, "レセ集計中・・・", "NotConnectSocket"));
                _calculateService.ReceFutanCalculateMain(new ReceCalculateRequest(inputData.PtIds, inputData.SeikyuYm, string.Empty));

                SendMessenger(new RecalculationStatus(false, CalculateStatusConstant.CheckErrorCheckBox, 0, 0, "レセチェック中・・・", "NotConnectSocket"));
                CheckErrorInMonth(inputData.HpId, inputData.UserId, inputData.SeikyuYm, inputData.PtIds);

                errorText = GetErrorTextAfterCheck(inputData.HpId, inputData.PtIds, inputData.SeikyuYm);

                SendMessenger(new RecalculationStatus(false, CalculateStatusConstant.ReceCheckCalculate, 0, 0, errorText, "NotConnectSocket"));
                _receiptRepository.UpdateReceStatus(inputData.ReceStatus, inputData.HpId, inputData.UserId);

                return new ReceiptCheckRecalculationOutputData(true);
            }
            catch (Exception ex)
            {
                _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
            finally
            {
                SendMessenger(new RecalculationStatus(true, CalculateStatusConstant.ReceCheckMessage, 0, 0, string.Empty, "NotConnectSocket"));

                _calculationInfRepository.ReleaseResource();
                _systemConfRepository.ReleaseResource();
                _receiptRepository.ReleaseResource();
                _tenantProvider.DisposeDataContext();
                _calculateService.ReleaseSource();
                _loggingHandler.Dispose();
                _commonMedicalCheck.ReleaseResource();
                _realtimeOrderErrorFinder.ReleaseResource();
            }
        }

        public void CheckErrorInMonth(int hpId, int userId, int seikyuYm, List<long> ptIds)
        {
            var allCheckCount = _calculationInfRepository.GetCountReceInfs(hpId, ptIds, seikyuYm);
            if (allCheckCount == 0) return;

            _receCheckOpts = _calculationInfRepository.GetReceCheckOpts(hpId);
            _receInfModels = _calculationInfRepository.GetReceInfModels(hpId, ptIds, seikyuYm);

            foreach (var receInfModel in _receInfModels)
            {
                _oldReceCheckErrs = _calculationInfRepository.ClearReceCmtErr(hpId, receInfModel.PtId, receInfModel.HokenId, receInfModel.SinYm);
                _sinKouiCounts = _calculationInfRepository.GetSinKouiCounts(hpId, receInfModel.PtId, receInfModel.SinYm, receInfModel.HokenId);

                CheckHoken(hpId, receInfModel);
                CheckByomei(hpId, receInfModel);
                CheckOrder(hpId, receInfModel);
                CheckRosai(hpId, receInfModel);
                CheckAftercare(hpId, receInfModel);
            }
            _calculationInfRepository.SaveChanged(hpId, userId, _newReceCheckErrs);
        }

        public void CheckHoken(int hpId, ReceInfModel receInfModel)
        {
            //expired
            if (_receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.ExpiredEndDateHokenErrCd) && _sinKouiCounts.Count > 0)
            {
                //hoken
                if (receInfModel.HokenId > 0 && receInfModel.Houbetu.AsInteger() != 0)
                {
                    //E1002 start date
                    var firstSinKouiCount = _sinKouiCounts.OrderBy(p => p.SinDate).FirstOrDefault(p => p.PtHokenPatterns.Any(q => q.HokenId == receInfModel.HokenId));
                    if (firstSinKouiCount != null && receInfModel.PtHokenInf.StartDate > 0 && receInfModel.PtHokenInf.StartDate > firstSinKouiCount.SinDate)
                    {
                        InsertReceCmtErr(hpId, receInfModel, ReceErrCdConst.ExpiredStartDateHokenErrCd,
                              ReceErrCdConst.ExpiredStartDateHokenErrMsg, "（" + CIUtil.SDateToShowSWDate(receInfModel.PtHokenInf.StartDate) + "～）", HOKEN_CHAR);
                    }

                    //E1001 end date
                    var lastSinKouiCount = _sinKouiCounts.OrderBy(p => p.SinDate).LastOrDefault(p => p.PtHokenPatterns.Any(q => q.HokenId == receInfModel.HokenId));
                    if (lastSinKouiCount != null && receInfModel.PtHokenInf.EndDate > 0 && receInfModel.PtHokenInf.EndDate < lastSinKouiCount.SinDate)
                    {
                        InsertReceCmtErr(hpId, receInfModel, ReceErrCdConst.ExpiredEndDateHokenErrCd,
                           ReceErrCdConst.ExpiredEndDateHokenErrMsg, "（～" + CIUtil.SDateToShowSWDate(receInfModel.PtHokenInf.EndDate) + "）", HOKEN_CHAR);
                    }
                }

                //kohi1
                if (receInfModel.Kohi1Id > 0 && receInfModel.Kohi1Houbetu.AsInteger() != 102)
                {
                    var firstSinKouiCount = _sinKouiCounts.OrderBy(p => p.SinDate).FirstOrDefault(p => p.PtHokenPatterns
                        .Any(q => q.Kohi1Id == receInfModel.Kohi1Id || q.Kohi2Id == receInfModel.Kohi1Id || q.Kohi3Id == receInfModel.Kohi1Id || q.Kohi4Id == receInfModel.Kohi1Id));
                    if (firstSinKouiCount != null && receInfModel.PtKohi1.StartDate > 0 && receInfModel.PtKohi1.StartDate > firstSinKouiCount.SinDate)
                    {
                        InsertReceCmtErr(hpId, receInfModel, ReceErrCdConst.ExpiredStartDateHokenErrCd,
                           ReceErrCdConst.ExpiredStartDateHokenErrMsg, "（" + CIUtil.SDateToShowSWDate(receInfModel.PtKohi1.StartDate) + "～）", KOHI1_CHAR);
                    }

                    var lastSinKouiCount = _sinKouiCounts.OrderBy(p => p.SinDate).LastOrDefault(p => p.PtHokenPatterns
                        .Any(q => q.Kohi1Id == receInfModel.Kohi1Id || q.Kohi2Id == receInfModel.Kohi1Id || q.Kohi3Id == receInfModel.Kohi1Id || q.Kohi4Id == receInfModel.Kohi1Id));
                    if (lastSinKouiCount != null && receInfModel.PtKohi1.EndDate > 0 && receInfModel.PtKohi1.EndDate < lastSinKouiCount.SinDate)
                    {
                        InsertReceCmtErr(hpId, receInfModel, ReceErrCdConst.ExpiredEndDateHokenErrCd,
                           ReceErrCdConst.ExpiredEndDateHokenErrMsg, "（～" + CIUtil.SDateToShowSWDate(receInfModel.PtKohi1.EndDate) + "）", KOHI1_CHAR);
                    }
                }

                //kohi2
                if (receInfModel.Kohi2Id > 0 && receInfModel.Kohi2Houbetu.AsInteger() != 102)
                {
                    var firstSinKouiCount = _sinKouiCounts.OrderBy(p => p.SinDate).FirstOrDefault(p => p.PtHokenPatterns
                        .Any(q => q.Kohi1Id == receInfModel.Kohi2Id || q.Kohi2Id == receInfModel.Kohi2Id || q.Kohi3Id == receInfModel.Kohi2Id || q.Kohi4Id == receInfModel.Kohi2Id));
                    if (firstSinKouiCount != null && receInfModel.PtKohi2.StartDate > 0 && receInfModel.PtKohi2.StartDate > firstSinKouiCount.SinDate)
                    {
                        InsertReceCmtErr(hpId, receInfModel, ReceErrCdConst.ExpiredStartDateHokenErrCd,
                           ReceErrCdConst.ExpiredStartDateHokenErrMsg, "（" + CIUtil.SDateToShowSWDate(receInfModel.PtKohi2.StartDate) + "～）", KOHI2_CHAR);
                    }

                    var lastSinKouiCount = _sinKouiCounts.OrderBy(p => p.SinDate).LastOrDefault(p => p.PtHokenPatterns
                        .Any(q => q.Kohi1Id == receInfModel.Kohi2Id || q.Kohi2Id == receInfModel.Kohi2Id || q.Kohi3Id == receInfModel.Kohi2Id || q.Kohi4Id == receInfModel.Kohi2Id));
                    if (lastSinKouiCount != null && receInfModel.PtKohi2.EndDate > 0 && receInfModel.PtKohi2.EndDate < lastSinKouiCount.SinDate)
                    {
                        InsertReceCmtErr(hpId, receInfModel, ReceErrCdConst.ExpiredEndDateHokenErrCd,
                           ReceErrCdConst.ExpiredEndDateHokenErrMsg, "（～" + CIUtil.SDateToShowSWDate(receInfModel.PtKohi2.EndDate) + "）", KOHI2_CHAR);
                    }
                }

                //kohi3
                if (receInfModel.Kohi3Id > 0 && receInfModel.Kohi3Houbetu.AsInteger() != 102)
                {
                    var firstSinKouiCount = _sinKouiCounts.OrderBy(p => p.SinDate).FirstOrDefault(p => p.PtHokenPatterns
                        .Any(q => q.Kohi1Id == receInfModel.Kohi3Id || q.Kohi2Id == receInfModel.Kohi3Id || q.Kohi3Id == receInfModel.Kohi3Id || q.Kohi4Id == receInfModel.Kohi3Id));
                    if (firstSinKouiCount != null && receInfModel.PtKohi3.StartDate > 0 && receInfModel.PtKohi3.StartDate > firstSinKouiCount.SinDate)
                    {
                        InsertReceCmtErr(hpId, receInfModel, ReceErrCdConst.ExpiredStartDateHokenErrCd,
                           ReceErrCdConst.ExpiredStartDateHokenErrMsg, "（" + CIUtil.SDateToShowSWDate(receInfModel.PtKohi3.StartDate) + "～）", KOHI3_CHAR);
                    }

                    var lastSinKouiCount = _sinKouiCounts.OrderBy(p => p.SinDate).LastOrDefault(p => p.PtHokenPatterns
                        .Any(q => q.Kohi1Id == receInfModel.Kohi3Id || q.Kohi2Id == receInfModel.Kohi3Id || q.Kohi3Id == receInfModel.Kohi3Id || q.Kohi4Id == receInfModel.Kohi3Id));
                    if (lastSinKouiCount != null && receInfModel.PtKohi3.EndDate > 0 && receInfModel.PtKohi3.EndDate < lastSinKouiCount.SinDate)
                    {
                        InsertReceCmtErr(hpId, receInfModel, ReceErrCdConst.ExpiredEndDateHokenErrCd,
                            ReceErrCdConst.ExpiredEndDateHokenErrMsg, "（～" + CIUtil.SDateToShowSWDate(receInfModel.PtKohi3.EndDate) + "）", KOHI3_CHAR);
                    }
                }

                //kohi4
                if (receInfModel.Kohi4Id > 0 && receInfModel.Kohi4Houbetu.AsInteger() != 102)
                {
                    var firstSinKouiCount = _sinKouiCounts.OrderBy(p => p.SinDate).FirstOrDefault(p => p.PtHokenPatterns
                        .Any(q => q.Kohi1Id == receInfModel.Kohi4Id || q.Kohi2Id == receInfModel.Kohi4Id || q.Kohi3Id == receInfModel.Kohi4Id || q.Kohi4Id == receInfModel.Kohi4Id));
                    if (firstSinKouiCount != null && receInfModel.PtKohi4.StartDate > 0 && receInfModel.PtKohi4.StartDate > firstSinKouiCount.SinDate)
                    {
                        InsertReceCmtErr(hpId, receInfModel, ReceErrCdConst.ExpiredStartDateHokenErrCd,
                           ReceErrCdConst.ExpiredStartDateHokenErrMsg, "（" + CIUtil.SDateToShowSWDate(receInfModel.PtKohi4.StartDate) + "～）", KOHI4_CHAR);
                    }

                    var lastSinKouiCount = _sinKouiCounts.OrderBy(p => p.SinDate).LastOrDefault(p => p.PtHokenPatterns
                        .Any(q => q.Kohi1Id == receInfModel.Kohi4Id || q.Kohi2Id == receInfModel.Kohi4Id || q.Kohi3Id == receInfModel.Kohi4Id || q.Kohi4Id == receInfModel.Kohi4Id));
                    if (lastSinKouiCount != null && receInfModel.PtKohi4.EndDate > 0 && receInfModel.PtKohi4.EndDate < lastSinKouiCount.SinDate)
                    {
                        InsertReceCmtErr(hpId, receInfModel, ReceErrCdConst.ExpiredEndDateHokenErrCd,
                           ReceErrCdConst.ExpiredEndDateHokenErrMsg, "（～" + CIUtil.SDateToShowSWDate(receInfModel.PtKohi4.EndDate) + "）", KOHI4_CHAR);
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
                    InsertReceCmtErr(hpId, receInfModel, ReceErrCdConst.UnConfirmedHokenErrCd,
                           ReceErrCdConst.UnConfirmedHokenErrMsg, "（最終確認: " + latestConfirmedDate + "）", HOKEN_CHAR);
                }
                if (receInfModel.Kohi1Id > 0 && !receInfModel.IsKohi1Confirmed)
                {
                    string latestConfirmedDate = string.Empty;
                    if (receInfModel.Kohi1Checks?.Count > 0)
                    {
                        latestConfirmedDate = CIUtil.SDateToShowSWDate(receInfModel.Kohi1Checks.OrderByDescending(p => p.ConfirmDate).FirstOrDefault()?.ConfirmDate ?? 0);
                    }
                    InsertReceCmtErr(hpId, receInfModel, ReceErrCdConst.UnConfirmedHokenErrCd,
                            ReceErrCdConst.UnConfirmedHokenErrMsg, "（最終確認: " + latestConfirmedDate + "）", KOHI1_CHAR);
                }
                if (receInfModel.Kohi2Id > 0 && !receInfModel.IsKohi2Confirmed)
                {
                    string latestConfirmedDate = string.Empty;
                    if (receInfModel.Kohi2Checks?.Count > 0)
                    {
                        latestConfirmedDate = CIUtil.SDateToShowSWDate(receInfModel.Kohi2Checks.OrderByDescending(p => p.ConfirmDate).FirstOrDefault()?.ConfirmDate ?? 0);
                    }
                    InsertReceCmtErr(hpId, receInfModel, ReceErrCdConst.UnConfirmedHokenErrCd,
                           ReceErrCdConst.UnConfirmedHokenErrMsg, "（最終確認: " + latestConfirmedDate + "）", KOHI2_CHAR);
                }
                if (receInfModel.Kohi3Id > 0 && !receInfModel.IsKohi3Confirmed)
                {
                    string latestConfirmedDate = string.Empty;
                    if (receInfModel.Kohi3Checks?.Count > 0)
                    {
                        latestConfirmedDate = CIUtil.SDateToShowSWDate(receInfModel.Kohi3Checks.OrderByDescending(p => p.ConfirmDate).FirstOrDefault()?.ConfirmDate ?? 0);
                    }
                    InsertReceCmtErr(hpId, receInfModel, ReceErrCdConst.UnConfirmedHokenErrCd,
                           ReceErrCdConst.UnConfirmedHokenErrMsg, "（最終確認: " + latestConfirmedDate + "）", KOHI3_CHAR);
                }
                if (receInfModel.Kohi4Id > 0 && !receInfModel.IsKohi4Confirmed)
                {
                    string latestConfirmedDate = string.Empty;
                    if (receInfModel.Kohi4Checks?.Count > 0)
                    {
                        latestConfirmedDate = CIUtil.SDateToShowSWDate(receInfModel.Kohi4Checks.OrderByDescending(p => p.ConfirmDate).FirstOrDefault()?.ConfirmDate ?? 0);
                    }
                    InsertReceCmtErr(hpId, receInfModel, ReceErrCdConst.UnConfirmedHokenErrCd,
                           ReceErrCdConst.UnConfirmedHokenErrMsg, "（最終確認: " + latestConfirmedDate + "）", KOHI4_CHAR);
                }
            }
        }

        public void CheckByomei(int hpId, ReceInfModel receInfModel)
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
                        InsertReceCmtErr(hpId, receInfModel, ReceErrCdConst.NotExistByomeiErrCd, ReceErrCdConst.NotExistByomeiErrMsg, msg2, sinDate: sinKouiDetail.SinDate);
                    }
                    else
                    {
                        InsertReceCmtErr(hpId, receInfModel, ReceErrCdConst.NotExistByomeiErrCd, ReceErrCdConst.NotExistByomeiErrMsg);
                    }
                }

                //E2011 Bui Order Byomei
                if (_systemConfRepository.GetSettingValue(6003, 0, hpId) == 1 && _receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.BuiOrderByomeiErrCd))
                {
                    foreach (var (sinKouiCount, msgError, itemName) in from sinKouiCount in _sinKouiCounts
                                                                       let odrInfs = _calculationInfRepository.GetOdrInfsBySinDate(hpId, receInfModel.PtId, sinKouiCount.SinDate, receInfModel.HokenId)
                                                                       let buiOdrItemMsts = _calculationInfRepository.GetBuiOdrItemMsts(hpId)
                                                                       let buiOdrItemByomeiMsts = _calculationInfRepository.GetBuiOdrItemByomeiMsts(hpId)
                                                                       let msgErrors = CheckBuiOrderByomei(buiOdrItemMsts, buiOdrItemByomeiMsts, odrInfs, ptByomeis)
                                                                       where msgErrors.Count > 0
                                                                       from msgError in msgErrors
                                                                       let itemName = odrInfs.FirstOrDefault(p => p.ItemCd == msgError)?.ItemName ?? string.Empty
                                                                       select (sinKouiCount, msgError, itemName))
                    {
                        InsertReceCmtErr(hpId, receInfModel, ReceErrCdConst.BuiOrderByomeiErrCd,
                                                             ReceErrCdConst.BuiOrderByomeiErrMsg,
                                                             itemName + " : " +
                                                             CIUtil.SDateToShowSWDate(sinKouiCount.SinDate) + "）",
                                                             msgError, sinDate: sinKouiCount.SinDate);
                    }
                }

                //E2010 Bui Order Byomei
                if (_systemConfRepository.GetSettingValue(6003, 0, hpId) == 1 && _receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.ByomeiBuiOrderByomeiChekkuErrCd))
                {
                    foreach (var errorOdrInfDetail in _errorOdrInfDetails)
                    {
                        foreach (var msg in errorOdrInfDetail.Errors)
                        {
                            InsertReceCmtErr(hpId, receInfModel, ReceErrCdConst.ByomeiBuiOrderByomeiChekkuErrCd, ReceErrCdConst.ByomeiBuiOrderByomeiChekkuErrMsg, msg, aCd: errorOdrInfDetail.ItemName, bCd: errorOdrInfDetail.OdrKouiKbn.ToString(), sinDate: errorOdrInfDetail.SinDate);
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
                            InsertReceCmtErr(hpId, receInfModel, ReceErrCdConst.CheckReVisitContiByomeiErrCd, ReceErrCdConst.CheckReVisitContiByomeiErrMsg, msg2, sinDate: sinKouiDetail.SinDate);
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
                                        InsertReceCmtErr(hpId, receInfModel, ReceErrCdConst.CheckFirstVisit2003ByomeiErrCd, ReceErrCdConst.CheckFirstVisit2004ByomeiErrMsg,
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
                                InsertReceCmtErr(hpId, receInfModel, ReceErrCdConst.CheckFirstVisit2003ByomeiErrCd, ReceErrCdConst.CheckFirstVisit2003ByomeiErrMsg, msg2, sinDate: sinKouiDetail.SinDate);
                            }
                        }
                    }
                }



                //E2005 check if has not main byomei 
                if (_receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.HasNotMainByomeiErrCd) && !ptByomeis.Any(p => p.IsMain))
                {
                    InsertReceCmtErr(hpId, receInfModel, ReceErrCdConst.HasNotMainByomeiErrCd, ReceErrCdConst.HasNotMainByomeiErrMsg);
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
                            InsertReceCmtErr(hpId, receInfModel, ReceErrCdConst.InvalidByomeiErrCd, ReceErrCdConst.InvalidByomeiErrMsg,
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
                            string cutByomei = CIUtil.Copy(ptByomei.Byomei, 1, 84);
                            string msg2 = string.Format("({0}...: {1}/20文字)", cutByomei, ptByomei.Byomei.Length);
                            InsertReceCmtErr(hpId, receInfModel, ReceErrCdConst.FreeTextLengthByomeiErrCd, ReceErrCdConst.FreeTextLengthByomeiErrMsg, msg2, cutByomei);
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
                            InsertReceCmtErr(hpId, receInfModel, ReceErrCdConst.CheckSuspectedByomeiErrCd,
                               ReceErrCdConst.CheckSuspectedByomeiErrMsg.Replace("xx", receCheckOpt.CheckOpt.AsString()), msg2, cutByomei);
                        }
                    }
                }

                //E2012
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
                        InsertReceCmtErr(hpId, receInfModel, ReceErrCdConst.DuplicateByomeiCheckErrCd,
                                                                             ReceErrCdConst.DuplicateByomeiCheckErrMsg,
                                                                             "（" + ptByomei.Byomei + " : " + CIUtil.SDateToShowSWDate(ptByomei.StartDate) + "）",
                                                                             ptByomei.ByomeiCd, string.Join(string.Empty, ptByomei.GetAllSyusyokuCds().ToArray()));
                    }
                }



                bool checkByomeiResponding = _receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.HasNotByomeiWithOdrErrCd);
                bool checkBuiOrderByomei = _receCheckOpts.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.BuiOrderByomeiErrCd);
                if (checkByomeiResponding || checkBuiOrderByomei)
                {
                    foreach (var (sinKouiCount, odrInfs) in from sinKouiCount in _sinKouiCounts
                                                            let odrInfs = _calculationInfRepository.GetOdrInfsBySinDate(hpId, receInfModel.PtId, sinKouiCount.SinDate, receInfModel.HokenId)
                                                            select (sinKouiCount, odrInfs))
                    {
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
                                        InsertReceCmtErr(hpId, receInfModel, ReceErrCdConst.HasNotByomeiWithOdrErrCd,
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
                                    string itemName = odrInfs.FirstOrDefault(p => p.ItemCd == msgError)?.ItemName ?? string.Empty;
                                    InsertReceCmtErr(hpId, receInfModel, ReceErrCdConst.BuiOrderByomeiErrCd,
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
                    foreach (var errorOdrInfDetail in _errorOdrInfDetails)
                    {
                        foreach (var msg in errorOdrInfDetail.Errors)
                        {
                            InsertReceCmtErr(hpId, receInfModel, ReceErrCdConst.ByomeiBuiOrderByomeiChekkuErrCd, ReceErrCdConst.ByomeiBuiOrderByomeiChekkuErrMsg, msg, aCd: errorOdrInfDetail.ItemName, bCd: errorOdrInfDetail.OdrKouiKbn.ToString(), sinDate: errorOdrInfDetail.SinDate);
                        }
                    }
                }
            }
        }

        public void CheckOrder(int hpId, ReceInfModel receInfModel)
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
                    foreach (var odrDetail in odrInfModel.OrdInfDetails)
                    {
                        todayOdrDetails.Add(new OrdInfoDetailModel(string.Empty, odrDetail.SinKouiKbn, odrDetail.ItemCd, odrDetail.ItemName, odrDetail.Suryo,
                                                                   odrDetail.UnitName, odrDetail.TermVal, odrDetail.SyohoKbn, odrDetail.SyohoLimitKbn,
                                                                   odrDetail.DrugKbn, odrDetail.SyohoKbn, odrDetail.IpnCd, odrDetail.Bunkatu, odrDetail.MasterSbt, odrDetail.BunkatuKoui));
                    }

                    var todayOdrInf = new OrdInfoModel(odrInfModel.OdrKouiKbn, odrInfModel.SanteiKbn, todayOdrDetails);
                    var resultOdrs = CheckOnlyDayLimitOrder(todayOdrInf, hpId, odrInfModel.PtId, odrInfModel.SinDate);
                    foreach (var odr in resultOdrs)
                    {
                        string msg2 = string.Format("（{0}: {1} [{2}日/{3}日]）", odr.ItemName, CIUtil.SDateToShowSWDate(odrInfModel.SinDate), odr.UsingDay, odr.LimitDay);
                        InsertReceCmtErr(hpId, receInfModel, ReceErrCdConst.ExceededDosageOdrErrCd, ReceErrCdConst.ExceededDosageOdrErrMsg,
                                                       msg2, odr.ItemCd, sinDate: odrInfModel.SinDate);
                    }
                }
                OrdInfDetailModels.AddRange(odrInfModel.OrdInfDetails);
            }

            #region Duplicate check
            if (isCheckDuplicateOdr)
            {
                List<string> checkedOdrItemCds = new();
                foreach (var odrDetail in OrdInfDetailModels)
                {
                    //E4002 check order with same effect
                    if (isCheckDuplicateOdr && odrDetail.IsDrugOrInjection && !string.IsNullOrEmpty(odrDetail.YjCd))
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
                                InsertReceCmtErr(hpId, receInfModel, ReceErrCdConst.DuplicateOdrErrCd, ReceErrCdConst.DuplicateOdrErrMsg,
                                                               msg2, odrDetail.ItemCd, sinDate: odrDetail.SinDate);
                            }
                            checkedOdrItemCds.Add(odrDetail.ItemCd);
                            checkedOdrItemCds.Add(duplicatedOdr.ItemCd);
                        }
                    }
                }
            }

            #endregion

            #region Expired check
            //E3001,E3002 check expired end date and start date
            if (isCheckExpiredOdr)
            {
                List<string> checkedItemCds = new();
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
                                InsertReceCmtErr(hpId, receInfModel, ReceErrCdConst.ExpiredEndDateOdrErrCd, ReceErrCdConst.ExpiredEndDateOdrErrMsg, msg2, itemCd, sinDate: sinKouiCount.SinDate);
                            }

                            var firstTenMst = _calculationInfRepository.FindFirstTenMst(hpId, itemCd);
                            if (firstTenMst != null && sinKouiCount.SinDate < firstTenMst.StartDate)
                            {
                                string msg2 = string.Format("（{0} {1}: {2}～）", sinKouiDetailModel.ItemName, CIUtil.SDateToShowSWDate(sinKouiCount.SinDate), CIUtil.SDateToShowSWDate(firstTenMst.StartDate));
                                InsertReceCmtErr(hpId, receInfModel, ReceErrCdConst.ExpiredStartDateOdrErrCd, ReceErrCdConst.ExpiredStartDateOdrErrMsg, msg2, itemCd, sinDate: sinKouiCount.SinDate);
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

                                List<int> results = new();

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

                                if (_systemConfRepository.GetSettingValue(3013, 0, hpId) == 1)
                                {
                                    // すべて同一に考える
                                    results.Add(2);
                                }

                                return results;
                            }
                            #endregion

                            List<DensiSanteiKaisuModel> densiSanteiKaisuModels = _calculationInfRepository.FindDensiSanteiKaisuList(hpId, sinDate, itemCd);
                            if (hokenInf != null && (hokenInf.HokenKbn == 11 || hokenInf.HokenKbn == 12 || hokenInf.HokenKbn == 13
                                || (hokenInf.HokenKbn == 14 && _systemConfRepository.GetSettingValue(3001, 0, hpId) == 1)))
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

                                List<int> checkHokenKbnTmp = new();
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

                                List<int> checkSanteiKbnTmp = new();
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
                                    itemGrpMsts = _calculationInfRepository.FindItemGrpMst(hpId, sinDate, 1, densiSanteiKaisu.ItemGrpCd);
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
                                    santeiCount = _calculationInfRepository.SanteiCount(hpId, receInfModel.PtId, startDate, sinDate,
                                                                   sinDate, 0, itemCds, checkSanteiKbnTmp, checkHokenKbnTmp);
                                }

                                if (santeiCount > densiSanteiKaisu.MaxCount)
                                {
                                    string msg2 = string.Format("({0}: {1}回 [{2}回/{3}])", sinKouiDetailModel.ItemName, santeiCount, densiSanteiKaisu.MaxCount, sTerm);
                                    InsertReceCmtErr(hpId, receInfModel, ReceErrCdConst.SanteiCountCheckErrCd, ReceErrCdConst.SanteiCountCheckErrMsg, msg2, itemCd);
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
                StringBuilder msg2 = new();
                foreach (var sinKouiCount in _sinKouiCounts)
                {
                    if (sinKouiCount.IsFirstVisit)
                    {
                        //msg2 max length = 100
                        string formatSinDate = CIUtil.SDateToShowSWDate(sinKouiCount.SinDate);
                        if (!msg2.ToString().Contains(formatSinDate) && msg2.Length + formatSinDate.Length + 2 <= 100)
                        {
                            if (!string.IsNullOrEmpty(msg2.ToString()))
                            {
                                msg2.Append(", ");
                            }
                            msg2.Append(formatSinDate);
                        }
                        suryoSum += sinKouiCount.SinKouiDetailModels.Where(p => ReceErrCdConst.IsFirstVisitCd.Contains(p.ItemCd)).Sum(p => p.Suryo);
                    }
                }
                if (suryoSum > 1)
                {
                    InsertReceCmtErr(hpId, receInfModel, ReceErrCdConst.FirstExamFeeCheckErrCd, ReceErrCdConst.FirstExamFeeCheckErrMsg, msg2.ToString());
                }
            }

            //E3005 check tokuzai item
            if (isCheckTokuzaiItem)
            {
                foreach (var sinKouiCount in from sinKouiCount in _sinKouiCounts
                                             where sinKouiCount.SinKouiDetailModels.Any(p => p.ItemCd == ReceErrCdConst.TokuzaiItemCd)
                                             select sinKouiCount)
                {
                    InsertReceCmtErr(hpId, receInfModel, ReceErrCdConst.TokuzaiItemCheckErrCd, ReceErrCdConst.TokuzaiItemCheckErrMsg,
                                                                       "（2017(H29)/04/01～使用不可）", ReceErrCdConst.TokuzaiItemCd, sinDate: sinKouiCount.SinDate);
                }
            }

            //E3007 check patient age to use order
            if (isCheckItemAge)
            {
                List<string> checkedItemCds = new List<string>();
                int iBirthDay = receInfModel.PtInf.Birthday;
                foreach (var sinKouiCount in _sinKouiCounts)
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
                                InsertReceCmtErr(hpId, receInfModel, ReceErrCdConst.ItemAgeCheckErrCd, ReceErrCdConst.ItemAgeCheckErrMsg, msg2, itemCd, sinDate: sinKouiCount.SinDate);
                            }

                            checkedItemCds.Add(itemCd);
                        }
                    }
                }
            }

            //E3008 check comment into order
            if (isCheckComment)
            {
                List<SinKouiModel> listSinKoui = _calculationInfRepository.GetListSinKoui(hpId, receInfModel.PtId, receInfModel.SinYm, receInfModel.HokenId);
                List<string> listReceCmtItemCode = _calculationInfRepository.GetListReceCmtItemCode(hpId, receInfModel.PtId, receInfModel.SinYm, receInfModel.HokenId);

                List<SinKouiDetailModel> listItemCdOfMonth = new List<SinKouiDetailModel>();
                listSinKoui.ForEach((sinKoui) =>
                {
                    listItemCdOfMonth.AddRange(sinKoui.SinKouiDetailModels);
                });

                listSinKoui.ForEach((sinKoui) =>
                {
                    if (sinKoui.ExistItemWithCommentSelect)
                    {
                        var listItemWithCmtSelect = sinKoui.SinKouiDetailModels.Where(s => s.CmtSelectList != null && s.CmtSelectList.Count > 0).ToList();

                        listItemWithCmtSelect.ForEach((sinKouiDetail) =>
                        {
                            List<string> listItemCd = sinKoui.SinKouiDetailModels.Select(s => s.ItemCd).ToList();
                            sinKouiDetail.CmtSelectList.ForEach((cmtSelect) =>
                            {
                                cmtSelect.ListGroupComment.ForEach((groupComment) =>
                                {
                                    List<RecedenCmtSelectModel> filteredCmtSelect = groupComment.ItemCmtModels.Where(r => r.CondKbn == 1).ToList();
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
                                            if (recedenCmtSelect.CmtSbt == 3 && listItemCdOfMonth.Any(x => x.ItemCd == ItemCdConst.CommentJissiRekkyoItemNameDummy && x.CmtOpt == sinKouiDetail.ItemCd))
                                            {
                                                existCmtSelect = true;
                                                break;
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
                                            InsertReceCmtErr(hpId, receInfModel, ReceErrCdConst.CommentCheckErrCd, ReceErrCdConst.CommentCheckErrMsg, message, itemCd, cmtCd, 0);
                                        }
                                    }
                                });
                            });
                        });
                    }
                });
            }

            if (isCheckAdditionItem)
            {
                var addtionItems = _calculationInfRepository.GetAddtionItems(hpId, receInfModel.PtId, receInfModel.SinYm, receInfModel.HokenId);
                if (addtionItems.Count > 0)
                {
                    foreach (var item in addtionItems)
                    {
                        InsertReceCmtErr(hpId, receInfModel, ReceErrCdConst.AdditionItemErrCd, CIUtil.Copy(item.Text, 1, 100), "（" + CIUtil.SDateToShowSWDate(item.SinDate) + "）",
                           item.ItemCd + "," + item.DelItemCd, item.TermCnt + "," + item.TermSbt + "," + item.IsWarning, item.SinDate);
                    }
                }
            }
        }

        private void CheckRosai(int hpId, ReceInfModel receInfItem)
        {
            //check use normal hoken but order Rosai item
            //■健康保険のレセプトで労災項目がオーダーされています。
            if (receInfItem.HokenKbn == 1 || receInfItem.HokenKbn == 2)
            {
                List<string> orderRosaiErrors = new List<string>();
                foreach (var sinKouiCount in _sinKouiCounts)
                {
                    foreach (var sinKouiDetailModel in sinKouiCount.SinKouiDetailModels)
                    {
                        if (sinKouiDetailModel.MasterSbt == "R")
                        {
                            orderRosaiErrors.Add(string.Format("    {0}/{1} ID:{2} [{3}] {4}", receInfItem.SeikyuYm / 100, receInfItem.SeikyuYm % 100, receInfItem.PtNum, sinKouiDetailModel.TenMst.ItemCd, sinKouiDetailModel.ReceName));
                        }
                    }
                }
                if (orderRosaiErrors.Count > 0)
                {
                    orderRosaiErrors.Insert(0, "■健康保険のレセプトで労災項目がオーダーされています。");
                    foreach (var error in orderRosaiErrors)
                    {
                        ErrorText.Append(error + Environment.NewLine);
                    }
                }
            }

            // check rosai can using
            if (_systemConfRepository.GetSettingValue(100003, 0, hpId) == 1
                    && seikyuYm >= _systemConfRepository.GetSettingParams(100003, 0, hpId).AsInteger()
                    && (receInfItem.HokenKbn == 11 || receInfItem.HokenKbn == 12) //check using Rosai Receden
                    && receInfItem.IsPaperRece == 0)
            {
                // check error Rousai kantoku cd empty
                if (!_calculationInfRepository.IsKantokuCdValid(hpId, receInfItem.HokenId, receInfItem.PtId))
                {
                    InsertReceCmtErr(hpId, receInfItem, ReceErrCdConst.HasNotRousaiKantokuErrCd, ReceErrCdConst.HasNotRousaiKantokuErrMsg);
                }

                // check error Rousai Saigai
                if (receInfItem.PtHokenInf.RousaiSaigaiKbn != 1 &&
                    receInfItem.PtHokenInf.RousaiSaigaiKbn != 2)
                {
                    InsertReceCmtErr(hpId, receInfItem, ReceErrCdConst.HasNotSaigaiKbnErrCd, ReceErrCdConst.HasNotSaigaiKbnErrMsg);
                }

                // check error Syobyo
                if (receInfItem.PtHokenInf.RousaiSyobyoDate <= 0)
                {
                    InsertReceCmtErr(hpId, receInfItem, ReceErrCdConst.HasNotSyobyoDateErrCd, ReceErrCdConst.HasNotSyobyoDateErrMsg);
                }

                //check error SyobyoKeika
                if (!_calculationInfRepository.ExistSyobyoKeikaData(hpId, receInfItem.PtId, receInfItem.SinYm, receInfItem.HokenId))
                {
                    InsertReceCmtErr(hpId, receInfItem, ReceErrCdConst.HasNotSyobyoKeikaErrCd, ReceErrCdConst.HasNotSyobyoKeikaErrMsg);
                }

            }

            //check use normal hoken but order Rosai item
            //■健康保険のレセプトで労災項目がオーダーされています。
            if (receInfItem.HokenKbn == 1 || receInfItem.HokenKbn == 2)
            {

                foreach (var sinKouiCount in _sinKouiCounts)
                {
                    foreach (var sinKouiDetailModel in sinKouiCount.SinKouiDetailModels)
                    {
                        if (sinKouiDetailModel.MasterSbt == "R")
                        {
                            string msg2 = string.Format("（{0}: {1}）", sinKouiDetailModel.ItemName, CIUtil.SDateToShowSWDate(sinKouiCount.SinDate));
                            InsertReceCmtErr(hpId, receInfItem, ReceErrCdConst.HokenUsingRosaiItemErrCd, ReceErrCdConst.HokenUsingRosaiItemErrMsg, msg2,
                                                           sinKouiDetailModel.ItemCd, sinKouiDetailModel.ItemName, sinDate: sinKouiCount.SinDate);
                        }
                    }
                }

            }
        }

        private void CheckAftercare(int hpId, ReceInfModel receInfItem)
        {
            // check aftercare can using
            if (_systemConfRepository.GetSettingValue(100003, 1, hpId) == 1 && seikyuYm >= _systemConfRepository.GetSettingParams(100003, 0, hpId).AsInteger() && receInfItem.HokenKbn == 13 && receInfItem.IsPaperRece == 0 && !_calculationInfRepository.ExistSyobyoKeikaData(hpId, receInfItem.PtId, receInfItem.SinYm, receInfItem.HokenId))
            {
                InsertReceCmtErr(hpId, receInfItem, ReceErrCdConst.HasNotSyobyoKeikaErrCd, ReceErrCdConst.HasNotSyobyoKeikaErrMsg);
            }
        }

        public List<string> CheckBuiOrderByomei(List<BuiOdrItemMstModel> buiOdrItemMsts, List<BuiOdrItemByomeiMstModel> buiOdrItemByomeiMsts, List<OrdInfDetailModel> todayOrderInfModels, List<PtDiseaseModel> ptByomeiModels)
        {
            List<string> msgErrors = new();
            foreach (var todayOrderInfModel in todayOrderInfModels)
            {
                if (!buiOdrItemMsts.Any(p => p.ItemCd == todayOrderInfModel.ItemCd))
                {
                    continue;
                }
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
                        else if (buiOdrByomeiMst.LrKbn == 0 && buiOdrByomeiMst.BothKbn == 1 && (ptByomeiModels.Any(p => (p.ByomeiHankToZen.AsString().Contains(BOTH) || p.ByomeiHankToZen.AsString().Contains(LEFT_RIGHT) || p.ByomeiHankToZen.AsString().Contains(RIGHT_LEFT))
                                && buiOdrByomeiMsts.Any(q => p.Byomei.Contains(HenkanJ.Instance.ToFullsize(q.ByomeiBui))))))
                        {
                            hasError = false;
                            break;
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

            _errorOdrInfDetails = new();
            List<string> errorMsgs = new();
            foreach (var odrInf in odrInfModels)
            {
                var OrdInfDetailModels = odrInf.OrdInfDetails.Where(x => string.IsNullOrEmpty(x.ItemCd) || x.ItemCd.Length == 4 || x.SinKouiKbn == 99);
                foreach (var detail in OrdInfDetailModels)
                {

                    List<BuiOdrMstModel> buiOdrMstCheckList = new();
                    List<BuiOdrMstModel> filteredBuiOdrMsts = new();
                    string compareName = IsSpecialComment(detail) ? detail.ItemName.Replace(detail.CmtName, "") : detail.ItemName;
                    compareName = HenkanJ.Instance.ToFullsize(compareName);
                    List<BuiOdrMstModel> buiOdrMstContainItemNames = new();
                    foreach (var buiOdrMst in buiOdrMsts)
                    {
                        List<string> odrBuiPatterns = new();
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

                        foreach (var _ in from pattern in odrBuiPatterns
                                          where compareName.Contains(HenkanJ.Instance.ToFullsize(pattern))
                                          select new { })
                        {
                            buiOdrMstContainItemNames.Add(buiOdrMst);
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
                    if (buiOdrMstWithMaxLength == null)
                    {
                        continue;
                    }
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
                            foreach (var _ in from mst in filteredBuiOdrByomeiMsts
                                              where HenkanJ.Instance.ToFullsize(ptByomei.Byomei).Contains(HenkanJ.Instance.ToFullsize(mst.ByomeiBui))
                                              select new { })
                            {
                                ptByomeisContainByomeiBui.Add(ptByomei);
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
                            if (!_errorOdrInfDetails.Any(d => d.OdrInfDetail == detail))
                            {
                                _errorOdrInfDetails.Add(new BuiErrorModel(detail, odrInf.OdrKouiKbn, odrInf.SinDate, output));
                            }
                            _errorOdrInfDetails.First(x => x.OdrInfDetail == detail).Errors.Add(msg2);
                        }
                    }
                }
            }
            return errorMsgs;
        }

        private bool CheckDuplicateByomei(bool checkDuplicateByomei, bool checkDuplicateSyusyokuByomei, PtDiseaseModel currentPtByomeiModel, PtDiseaseModel comparedPtByomeiModel, int recehokenId)
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

        private bool ValidateByomeiReflectOdrSite(string buiOdr, string byomeiName, int LrKbn, int BothKbn)
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

        private string GetErrorTextAfterCheck(int hpId, List<long> ptIds, int seikyuYm, bool notUsingReceInfCache = false)
        {
            if (_receInfModels == null || notUsingReceInfCache)
            {
                _receInfModels = _calculationInfRepository.GetReceInfModels(hpId, ptIds, seikyuYm);
            }

            List<string> errors = new();
            foreach (var receInfModel in _receInfModels)
            {
                if (receInfModel.IsPaperRece == 1 || receInfModel.SeikyuKbn == 2 || receInfModel.HokenKbn == 0 || receInfModel.HokenKbn == 14
                        || ((_systemConfRepository.GetSettingValue(100003, 0, hpId) != 1 || (_systemConfRepository.GetSettingValue(100003, 0, hpId) == 1 && receInfModel.SeikyuYm < _systemConfRepository.GetSettingParams(100003, 0, hpId).AsInteger()))
                        && (receInfModel.HokenKbn == 11 || receInfModel.HokenKbn == 12))
                        || ((_systemConfRepository.GetSettingValue(100003, 1, hpId) != 1
                        || (_systemConfRepository.GetSettingValue(100003, 1, hpId) == 1 && receInfModel.SeikyuYm < _systemConfRepository.GetSettingParams(100003, 1, hpId).AsInteger()))
                        && receInfModel.HokenKbn == 13))
                {
                    continue;
                }
                var sinKouiCounts = _calculationInfRepository.GetSinKouiCounts(hpId, receInfModel.PtId, receInfModel.SinYm, receInfModel.HokenId);
                foreach (var sinKouiCount in sinKouiCounts)
                {
                    foreach (var sinKouiDetailModel in sinKouiCount.SinKouiDetailModels)
                    {
                        if (string.IsNullOrWhiteSpace(sinKouiDetailModel.ItemCd))
                        {
                            continue;
                        }

                        if (sinKouiDetailModel.IsNodspRece == 1)
                        {
                            continue;
                        }

                        if (!new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "S", "W", "@", "Z", "K" }.Contains(sinKouiDetailModel.ItemCd.Substring(0, 1)))
                        {
                            errors.Add(string.Format("    {0}/{1} ID:{2} [{3}] {4}", seikyuYm / 100, seikyuYm % 100, receInfModel.PtInf.PtNum, sinKouiDetailModel.ItemCd, sinKouiDetailModel.ItemName));
                        }
                    }
                }
            }
            if (errors.Count > 0)
            {
                errors.Insert(0, "■請求できない項目がオーダーされています。");
                foreach (var error in errors)
                {
                    ErrorText.Append(error);
                    ErrorText.Append(Environment.NewLine);
                }
            }

            //check use Rosai Receden but not set 災害区分
            //■災害区分が設定されていません。
            List<string> rosaiRecedenErrors = new List<string>();
            if ((int)_systemConfRepository.GetSettingValue(100003, 0, hpId) == 1 && seikyuYm >= _systemConfRepository.GetSettingParams(100003, 0, hpId).AsInteger())
            {
                var rosaiRecedenPts = _receInfModels.FindAll(p => (p.HokenKbn == 11 || p.HokenKbn == 12) && p.IsPaperRece == 0);

                foreach (var rosaiRecedenPt in rosaiRecedenPts)
                {
                    if (rosaiRecedenPt.PtHokenInf.RousaiSaigaiKbn == 1 || rosaiRecedenPt.PtHokenInf.RousaiSaigaiKbn == 2) continue;

                    rosaiRecedenErrors.Add(string.Format("    {0}/{1} ID:{2} [保険:{3}]", seikyuYm / 100, seikyuYm % 100, rosaiRecedenPt.PtInf.PtNum, rosaiRecedenPt.HokenId));
                }

                if (rosaiRecedenErrors.Count > 0)
                {
                    rosaiRecedenErrors.Insert(0, "■災害区分が設定されていません。");
                    foreach (var error in rosaiRecedenErrors)
                    {
                        ErrorText.Append(error);
                        ErrorText.Append(Environment.NewLine);
                    }
                }
            }

            //check exist data in RECE_SEIKYU but not exist in RECE_INF
            //■返戻/月遅れ登録に誤りがあるため、レセプトを作成できません。
            List<string> receSeiKyuErrors = new List<string>();
            var receSeiKyuModels = _calculationInfRepository.GetReceSeikyus(hpId, ptIds, seikyuYm);
            foreach (var receSeiKyuModel in receSeiKyuModels)
            {
                if (!_receInfModels.Any(p => p.PtId == receSeiKyuModel.PtId && p.SinYm == receSeiKyuModel.SinYm
                                         && (p.HokenId == receSeiKyuModel.HokenId || p.HokenId2 == receSeiKyuModel.HokenId)))
                {
                    receSeiKyuErrors.Add(string.Format("    {0}/{1} ID:{2} [保険:{3}] {4}",
                        receSeiKyuModel.SinYm / 100, receSeiKyuModel.SinYm % 100, receSeiKyuModel.PtNum, receSeiKyuModel.HokenId, receSeiKyuModel.SeikyuKbn));
                }
            }
            if (receSeiKyuErrors.Count > 0)
            {
                receSeiKyuErrors.Insert(0, "■返戻/月遅れ登録に誤りがあるため、レセプトを作成できません。");
                foreach (var error in receSeiKyuErrors)
                {
                    ErrorText.Append(error);
                    ErrorText.Append(Environment.NewLine);
                }
            }

            //check patient ZaiganIso(在がん医総）
            //■週単位計算項目　次月に月またぎで算定要件(暦週)を満たしています。
            //診療内容を確認してください。
            if (_systemConfRepository.GetSettingValue(2028, 0, hpId) == 1)
            {
                DateTime firstDateOfMonth = CIUtil.IntToDate(seikyuYm * 100 + 1);
                var lastDateOfMonth = new DateTime(firstDateOfMonth.Year, firstDateOfMonth.Month, DateTime.DaysInMonth(firstDateOfMonth.Year, firstDateOfMonth.Month));
                var zaiganIsoItems = _calculationInfRepository.GetZaiganIsoItems(hpId, seikyuYm);
                if (zaiganIsoItems.Count > 0)
                {

                    //check part of next month
                    if (lastDateOfMonth.DayOfWeek < DayOfWeek.Wednesday)
                    {
                        List<string> santeiNextMonthErrors = new List<string>();
                        var kouiDetails = _calculationInfRepository.GetKouiDetailToCheckSantei(hpId, ptIds, seikyuYm, zaiganIsoItems.Select(p => p.ItemCd).ToList(), true);
                        var keysGroupBy = kouiDetails.GroupBy(p => new { p.PtId, p.SinYm, p.ItemCd }).Select(p => p.FirstOrDefault());
                        if (keysGroupBy != null)
                        {
                            foreach (var key in keysGroupBy)
                            {
                                if (kouiDetails.Count(p => p.PtId == (key != null ? key.PtId : 0) && p.SinYm == (key != null ? key.SinYm : 0) && p.ItemCd == (key != null ? key.ItemCd : string.Empty)) >= 4)
                                {
                                    int santeiStartDate = _calculationInfRepository.GetSanteiStartDate(hpId, key != null ? key.PtId : 0, seikyuYm);
                                    if (_calculationInfRepository.HasErrorWithSanteiByStartDate(hpId, key != null ? key.PtId : 0, seikyuYm, santeiStartDate, key != null ? key.ItemCd : string.Empty))
                                    {
                                        var sinKouiDetail = kouiDetails.FirstOrDefault(p => p.PtId == (key != null ? key.PtId : 0) && p.SinYm == (key != null ? key.SinYm : 0) && p.ItemCd == (key != null ? key.ItemCd : string.Empty)) ?? new();
                                        santeiNextMonthErrors.Add(string.Format("    {0}/{1} ID:{2} [{3}] {4}", seikyuYm / 100, seikyuYm % 100, sinKouiDetail.PtNum, sinKouiDetail.ItemCd, sinKouiDetail.ReceName));
                                    }
                                }
                            }
                            if (santeiNextMonthErrors.Count > 0)
                            {
                                santeiNextMonthErrors.Insert(0, "■週単位計算項目　次月に月またぎで算定要件(暦週)を満たしています。" +
                                                                Environment.NewLine + "    診療内容を確認してください。");
                                foreach (var error in santeiNextMonthErrors)
                                {
                                    ErrorText.Append(error);
                                    ErrorText.Append(Environment.NewLine);
                                }
                            }
                        }
                    }

                    //check part of last month
                    if (firstDateOfMonth.DayOfWeek > DayOfWeek.Wednesday)
                    {
                        List<string> santeiLastMonthErrors = new();
                        var kouiDetails = _calculationInfRepository.GetKouiDetailToCheckSantei(hpId, ptIds, seikyuYm, zaiganIsoItems.Select(p => p.ItemCd).ToList(), false);
                        var keysGroupBy = kouiDetails.GroupBy(p => new { p.PtId, p.SinYm, p.ItemCd }).Select(p => p.FirstOrDefault());
                        if (keysGroupBy != null)
                        {
                            foreach (var key in keysGroupBy)
                            {
                                if (kouiDetails.Count(p => p.PtId == (key != null ? key.PtId : 0) && p.SinYm == (key != null ? key.SinYm : 0) && p.ItemCd == (key != null ? key.ItemCd : string.Empty)) >= 4)
                                {
                                    int santeiEndDate = _calculationInfRepository.GetSanteiEndDate(hpId, key != null ? key.PtId : 0, seikyuYm);
                                    if (_calculationInfRepository.HasErrorWithSanteiByEndDate(hpId, key?.PtId ?? 0, seikyuYm, santeiEndDate, key?.ItemCd ?? string.Empty))
                                    {
                                        var sinKouiDetail = kouiDetails.FirstOrDefault(p => p.PtId == (key?.PtId ?? 0) && p.SinYm == (key?.SinYm ?? 0) && p.ItemCd == (key?.ItemCd ?? string.Empty)) ?? new();
                                        santeiLastMonthErrors.Add(string.Format("    {0}/{1} ID:{2} [{3}] {4}", seikyuYm / 100, seikyuYm % 100, sinKouiDetail.PtNum, sinKouiDetail.ItemCd, sinKouiDetail.ReceName));
                                    }
                                }
                            }
                            if (santeiLastMonthErrors.Count > 0)
                            {
                                santeiLastMonthErrors.Insert(0, "■週単位計算項目　前月に月またぎで算定要件(暦週)を満たしています。" +
                                                                Environment.NewLine + "    診療内容を確認してください。");
                                foreach (var error in santeiLastMonthErrors)
                                {
                                    ErrorText.Append(error);
                                    ErrorText.Append(Environment.NewLine);
                                }
                            }
                        }
                    }
                }
            }
            return ErrorText.ToString();
        }

        private List<DayLimitResultModel> CheckOnlyDayLimitOrder(OrdInfoModel todayOdrInfModel, int hpId, long ptId, int sinDate)
        {
            var dayLimitChecker = new DayLimitChecker<OrdInfoModel, OrdInfoDetailModel>();
            dayLimitChecker.HpID = hpId;
            dayLimitChecker.PtID = ptId;
            dayLimitChecker.Sinday = sinDate;

            var commonMedicalCheck = new CommonMedicalCheck(_tenantProvider, _realtimeOrderErrorFinder);

            commonMedicalCheck.InitUnitCheck(dayLimitChecker);

            var realtimeChecker = _commonMedicalCheck.CheckOnlyDayLimit(todayOdrInfModel);

            return realtimeChecker;
        }

        private void InsertReceCmtErr(int hpId, ReceInfModel receInfModel, string errCd, string errMsg1, string errMsg2 = "", string aCd = " ", string bCd = " ", int sinDate = 0)
        {
            if (!string.IsNullOrEmpty(errMsg1) && errMsg1.Length > 100)
            {
                errMsg1 = CIUtil.Copy(errMsg1, 1, 99) + "…";
            }
            if (!string.IsNullOrEmpty(errMsg2) && errMsg2.Length > 100)
            {
                errMsg2 = CIUtil.Copy(errMsg2, 1, 99) + "…";
            }

            var existNewReceCheckErr = _newReceCheckErrs.FirstOrDefault(p => p.HpId == hpId &&
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
                    existNewReceCheckErr.ChangeMessage1(errMsg1);
                    existNewReceCheckErr.ChangeMessage2(errMsg2);
                }
                return;
            }

            var newReceCheckErr = new ReceCheckErrModel(hpId, receInfModel.PtId, receInfModel.SinYm, receInfModel.HokenId, errCd, sinDate, aCd, bCd, errMsg1, errMsg2, 0);

            var existedReceCheckErr = _oldReceCheckErrs.FirstOrDefault(p => p.HpId == newReceCheckErr.HpId &&
                                                                            p.PtId == newReceCheckErr.PtId &&
                                                                            p.SinYm == newReceCheckErr.SinYm &&
                                                                            p.SinDate == newReceCheckErr.SinDate &&
                                                                            p.HokenId == newReceCheckErr.HokenId &&
                                                                            p.ErrCd == newReceCheckErr.ErrCd &&
                                                                            p.ACd == newReceCheckErr.ACd &&
                                                                            p.BCd == newReceCheckErr.BCd);
            if (existedReceCheckErr != null)
            {
                newReceCheckErr.ChangeIsChecked(existedReceCheckErr.IsChecked);
            }
            _newReceCheckErrs.Add(newReceCheckErr);
        }

        private void SendMessenger(RecalculationStatus status)
        {
            _messenger!.Send(status);
        }
    }
}
