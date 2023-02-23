using Domain.Models.Diseases;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Domain.Models.Receipt;
using Domain.Models.Receipt.Recalculation;
using Domain.Models.SystemConf;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using UseCase.Receipt.Recalculation;

namespace Interactor.Receipt;

public class RecalculationInteractor : IRecalculationInputPort
{
    private readonly IReceiptRepository _receiptRepository;
    private readonly ISystemConfRepository _systemConfRepository;
    private readonly IPtDiseaseRepository _ptDiseaseRepository;
    private readonly IOrdInfRepository _ordInfRepository;
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

    public RecalculationInteractor(IReceiptRepository receiptRepository, ISystemConfRepository systemConfRepository, IPtDiseaseRepository ptDiseaseRepository, IOrdInfRepository ordInfRepository)
    {
        _receiptRepository = receiptRepository;
        _systemConfRepository = systemConfRepository;
        _ptDiseaseRepository = ptDiseaseRepository;
        _ordInfRepository = ordInfRepository;
    }

    public RecalculationOutputData Handle(RecalculationInputData inputData)
    {
        try
        {
            List<ReceCheckErrModel> newReceCheckErrList = new();
            string errorMessage = string.Empty;
            var receCheckOptList = GetReceCheckOptModelList(inputData.HpId);
            var receRecalculationList = _receiptRepository.GetReceRecalculationList(inputData.HpId, inputData.SinYm, inputData.PtIdList);
            int allCheckCount = receRecalculationList.Count;
            foreach (var recalculationItem in receRecalculationList)
            {
                if (inputData.IsStopCalc)
                {
                    break;
                }
                List<BuiErrorModel> errorOdrInfDetails = new();
                var oldReceCheckErrList = _receiptRepository.GetReceCheckErrList(inputData.HpId, recalculationItem.SinYm, recalculationItem.PtId, recalculationItem.HokenId);
                var sinKouiCountList = _receiptRepository.GetSinKouiCountList(inputData.HpId, recalculationItem.SinYm, recalculationItem.PtId, recalculationItem.HokenId);
                newReceCheckErrList = CheckHokenError(recalculationItem, oldReceCheckErrList, newReceCheckErrList, receCheckOptList, sinKouiCountList);
                newReceCheckErrList = CheckByomeiError(inputData.HpId, recalculationItem, oldReceCheckErrList, newReceCheckErrList, receCheckOptList, sinKouiCountList, ref errorOdrInfDetails);
            }
            var count = newReceCheckErrList.Count;
            return new RecalculationOutputData(RecalculationStatus.Successed, errorMessage + count.ToString());
        }
        finally
        {
            _receiptRepository.ReleaseResource();
            _systemConfRepository.ReleaseResource();
            _ptDiseaseRepository.ReleaseResource();
            _ordInfRepository.ReleaseResource();
        }
    }

    #region Private funciton
    private List<ReceCheckOptModel> GetReceCheckOptModelList(int hpId)
    {
        var receCheckOptList = _receiptRepository.GetReceCheckOptList(hpId);
        if (!receCheckOptList.Any(p => p.ErrCd == ReceErrCdConst.ExpiredEndDateHokenErrCd))
        {
            receCheckOptList.Add(GetDefaultReceCheckOpt(ReceErrCdConst.ExpiredEndDateHokenErrCd));
        }
        if (!receCheckOptList.Any(p => p.ErrCd == ReceErrCdConst.UnConfirmedHokenErrCd))
        {
            receCheckOptList.Add(GetDefaultReceCheckOpt(ReceErrCdConst.UnConfirmedHokenErrCd));
        }
        if (!receCheckOptList.Any(p => p.ErrCd == ReceErrCdConst.NotExistByomeiErrCd))
        {
            receCheckOptList.Add(GetDefaultReceCheckOpt(ReceErrCdConst.NotExistByomeiErrCd));
        }
        if (!receCheckOptList.Any(p => p.ErrCd == ReceErrCdConst.CheckFirstVisit2003ByomeiErrCd))
        {
            receCheckOptList.Add(GetDefaultReceCheckOpt(ReceErrCdConst.CheckFirstVisit2003ByomeiErrCd));
        }
        if (!receCheckOptList.Any(p => p.ErrCd == ReceErrCdConst.HasNotMainByomeiErrCd))
        {
            receCheckOptList.Add(GetDefaultReceCheckOpt(ReceErrCdConst.HasNotMainByomeiErrCd));
        }
        if (!receCheckOptList.Any(p => p.ErrCd == ReceErrCdConst.InvalidByomeiErrCd))
        {
            receCheckOptList.Add(GetDefaultReceCheckOpt(ReceErrCdConst.InvalidByomeiErrCd));
        }
        if (!receCheckOptList.Any(p => p.ErrCd == ReceErrCdConst.FreeTextLengthByomeiErrCd))
        {
            receCheckOptList.Add(GetDefaultReceCheckOpt(ReceErrCdConst.FreeTextLengthByomeiErrCd));
        }
        if (!receCheckOptList.Any(p => p.ErrCd == ReceErrCdConst.CheckSuspectedByomeiErrCd))
        {
            receCheckOptList.Add(GetDefaultReceCheckOpt(ReceErrCdConst.CheckSuspectedByomeiErrCd));
        }
        if (!receCheckOptList.Any(p => p.ErrCd == ReceErrCdConst.HasNotByomeiWithOdrErrCd))
        {
            receCheckOptList.Add(GetDefaultReceCheckOpt(ReceErrCdConst.HasNotByomeiWithOdrErrCd));
        }
        if (!receCheckOptList.Any(p => p.ErrCd == ReceErrCdConst.ExpiredEndDateOdrErrCd))
        {
            receCheckOptList.Add(GetDefaultReceCheckOpt(ReceErrCdConst.ExpiredEndDateOdrErrCd));
        }
        if (!receCheckOptList.Any(p => p.ErrCd == ReceErrCdConst.FirstExamFeeCheckErrCd))
        {
            receCheckOptList.Add(GetDefaultReceCheckOpt(ReceErrCdConst.FirstExamFeeCheckErrCd));
        }
        if (!receCheckOptList.Any(p => p.ErrCd == ReceErrCdConst.SanteiCountCheckErrCd))
        {
            receCheckOptList.Add(GetDefaultReceCheckOpt(ReceErrCdConst.SanteiCountCheckErrCd));
        }
        if (!receCheckOptList.Any(p => p.ErrCd == ReceErrCdConst.TokuzaiItemCheckErrCd))
        {
            receCheckOptList.Add(GetDefaultReceCheckOpt(ReceErrCdConst.TokuzaiItemCheckErrCd));
        }
        if (!receCheckOptList.Any(p => p.ErrCd == ReceErrCdConst.ItemAgeCheckErrCd))
        {
            receCheckOptList.Add(GetDefaultReceCheckOpt(ReceErrCdConst.ItemAgeCheckErrCd));
        }
        if (!receCheckOptList.Any(p => p.ErrCd == ReceErrCdConst.CommentCheckErrCd))
        {
            receCheckOptList.Add(GetDefaultReceCheckOpt(ReceErrCdConst.CommentCheckErrCd));
        }
        if (!receCheckOptList.Any(p => p.ErrCd == ReceErrCdConst.ExceededDosageOdrErrCd))
        {
            receCheckOptList.Add(GetDefaultReceCheckOpt(ReceErrCdConst.ExceededDosageOdrErrCd));
        }
        if (!receCheckOptList.Any(p => p.ErrCd == ReceErrCdConst.DuplicateOdrErrCd))
        {
            receCheckOptList.Add(GetDefaultReceCheckOpt(ReceErrCdConst.DuplicateOdrErrCd));
        }
        if (!receCheckOptList.Any(p => p.ErrCd == ReceErrCdConst.ByomeiBuiOrderByomeiChekkuErrCd))
        {
            receCheckOptList.Add(GetDefaultReceCheckOpt(ReceErrCdConst.ByomeiBuiOrderByomeiChekkuErrCd));
        }
        if (!receCheckOptList.Any(p => p.ErrCd == ReceErrCdConst.BuiOrderByomeiErrCd))
        {
            receCheckOptList.Add(GetDefaultReceCheckOpt(ReceErrCdConst.BuiOrderByomeiErrCd));
        }
        if (!receCheckOptList.Any(p => p.ErrCd == ReceErrCdConst.DuplicateByomeiCheckErrCd))
        {
            receCheckOptList.Add(GetDefaultReceCheckOpt(ReceErrCdConst.DuplicateByomeiCheckErrCd));
        }
        if (!receCheckOptList.Any(p => p.ErrCd == ReceErrCdConst.DuplicateSyusyokuByomeiCheckErrCd))
        {
            receCheckOptList.Add(GetDefaultReceCheckOpt(ReceErrCdConst.DuplicateSyusyokuByomeiCheckErrCd));
        }
        if (!receCheckOptList.Any(p => p.ErrCd == ReceErrCdConst.AdditionItemErrCd))
        {
            receCheckOptList.Add(GetDefaultReceCheckOpt(ReceErrCdConst.AdditionItemErrCd));
        }
        return receCheckOptList;
    }

    private ReceCheckOptModel GetDefaultReceCheckOpt(string errCd)
    {
        if (errCd == ReceErrCdConst.CheckSuspectedByomeiErrCd)
        {
            return new ReceCheckOptModel(errCd, 3);
        }
        return new ReceCheckOptModel(errCd);
    }

    private void AddReceCmtErrNew(List<ReceCheckErrModel> oldReceCheckErrList, List<ReceCheckErrModel> newReceCheckErrList, ReceRecalculationModel receRecalculationModel, string errCd, string errMsg1, string errMsg2 = "", string aCd = " ", string bCd = " ", int sinDate = 0)
    {
        if (!string.IsNullOrEmpty(errMsg1) && errMsg1.Length > 100)
        {
            errMsg1 = CIUtil.Copy(errMsg1, 1, 99) + "…";
        }
        if (!string.IsNullOrEmpty(errMsg2) && errMsg2.Length > 100)
        {
            errMsg2 = CIUtil.Copy(errMsg2, 1, 99) + "…";
        }

        var existNewReceCheckErr = newReceCheckErrList.FirstOrDefault(item => item.PtId == receRecalculationModel.PtId
                                                                              && item.SinYm == receRecalculationModel.SinYm
                                                                              && item.SinDate == sinDate
                                                                              && item.HokenId == receRecalculationModel.HokenId
                                                                              && item.ErrCd == errCd
                                                                              && item.ACd == aCd
                                                                              && item.BCd == bCd);

        if (existNewReceCheckErr != null)
        {
            if (errCd == ReceErrCdConst.SanteiCountCheckErrCd)
            {
                existNewReceCheckErr.ChangeMessage(errMsg1, errMsg2);
            }
            return;
        }

        var existedReceCheckErr = oldReceCheckErrList.FirstOrDefault(item => item.PtId == receRecalculationModel.PtId
                                                                            && item.SinYm == receRecalculationModel.SinYm
                                                                            && item.SinDate == sinDate
                                                                            && item.HokenId == receRecalculationModel.HokenId
                                                                            && item.ErrCd == errCd
                                                                            && item.ACd == aCd
                                                                            && item.BCd == bCd);
        int isChecked = 0;
        if (existedReceCheckErr != null)
        {
            isChecked = existedReceCheckErr.IsChecked;
        }

        var newReceCheckErr = new ReceCheckErrModel(
                                    receRecalculationModel.PtId,
                                    receRecalculationModel.SinYm,
                                    receRecalculationModel.HokenId,
                                    errCd,
                                    sinDate,
                                    aCd,
                                    bCd,
                                    errMsg1,
                                    errMsg2,
                                    isChecked
                                );

        newReceCheckErrList.Add(newReceCheckErr);
    }

    private List<string> CheckBuiOrderByomei(List<BuiOdrItemMstModel> buiOdrItemMsts, List<BuiOdrItemByomeiMstModel> buiOdrItemByomeiMsts, List<TodayOdrInfDetailModel> todayOrderInfModels, List<PtDiseaseModel> PtDiseaseModels)
    {
        List<string> msgErrors = new();
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
                        if (PtDiseaseModels.Any(p => buiOdrByomeiMsts.Any(q => p.ByomeiHankToZen.Contains(HenkanJ.HankToZen(q.ByomeiBui)))))
                        {
                            hasError = false;
                            break;
                        }
                    }
                    else if (buiOdrByomeiMst.LrKbn == 1 && buiOdrByomeiMst.BothKbn == 1)
                    {
                        if (PtDiseaseModels.Any(p => (p.ByomeiHankToZen.AsString().Contains(LEFT) || p.ByomeiHankToZen.AsString().Contains(RIGHT) ||
                            p.ByomeiHankToZen.AsString().Contains(BOTH)) && buiOdrByomeiMsts.Any(q => p.ByomeiHankToZen.Contains(HenkanJ.HankToZen(q.ByomeiBui)))))
                        {
                            hasError = false;
                            break;
                        }
                    }
                    else if (buiOdrByomeiMst.LrKbn == 1 && buiOdrByomeiMst.BothKbn == 0)
                    {
                        if (PtDiseaseModels.Any(p => (p.ByomeiHankToZen.AsString().Contains(LEFT) || p.ByomeiHankToZen.AsString().Contains(RIGHT))
                            && !p.ByomeiHankToZen.AsString().Contains(LEFT_RIGHT) && !p.ByomeiHankToZen.AsString().Contains(RIGHT_LEFT) && buiOdrByomeiMsts.Any(q => p.Byomei.Contains(HenkanJ.HankToZen(q.ByomeiBui)))))
                        {
                            hasError = false;
                            break;
                        }
                    }
                    else if (buiOdrByomeiMst.LrKbn == 0 && buiOdrByomeiMst.BothKbn == 1)
                    {
                        if (PtDiseaseModels.Any(p => (p.ByomeiHankToZen.AsString().Contains(BOTH) || p.ByomeiHankToZen.AsString().Contains(LEFT_RIGHT) || p.ByomeiHankToZen.AsString().Contains(RIGHT_LEFT))
                            && buiOdrByomeiMsts.Any(q => p.Byomei.Contains(HenkanJ.HankToZen(q.ByomeiBui)))))
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

    private List<BuiErrorModel> CheckByomeiWithBuiOdr(List<BuiErrorModel> errorOdrInfDetails, List<OrdInfModel> odrInfModelList, List<BuiOdrMstModel> buiOdrMstList, List<BuiOdrByomeiMstModel> buiOdrByomeiMstList, List<PtDiseaseModel> ptByomeiList)
    {
        bool IsSpecialComment(OrdInfDetailModel detail)
        {
            return detail.SinKouiKbn == 99 && !string.IsNullOrEmpty(detail.CmpOpt);
        }
        List<string> errorMsgs = new List<string>();
        foreach (var odrInf in odrInfModelList)
        {
            var odrInfDetailModels = odrInf.OrdInfDetails.Where(x => string.IsNullOrEmpty(x.ItemCd) || x.ItemCd.Length == 4 || x.SinKouiKbn == 99);
            foreach (var detail in odrInfDetailModels)
            {

                List<BuiOdrMstModel> buiOdrMstCheckList = new();
                List<BuiOdrMstModel> filteredBuiOdrMsts = new();
                string compareName = IsSpecialComment(detail) ? detail.ItemName.Replace(detail.CmtName, "") : detail.ItemName;
                compareName = HenkanJ.HankToZen(compareName);
                List<BuiOdrMstModel> buiOdrMstContainItemNames = new();
                foreach (var buiOdrMst in buiOdrMstList)
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
                        if (compareName.Contains(HenkanJ.HankToZen(pattern)))
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
                    var filteredBuiOdrByomeiMsts = buiOdrByomeiMstList.Where(mst => mst.BuiId == buiOdrMst.BuiId);
                    List<PtDiseaseModel> ptByomeisContainByomeiBui = new();
                    foreach (var ptByomei in ptByomeiList)
                    {
                        foreach (var mst in filteredBuiOdrByomeiMsts)
                        {
                            if (HenkanJ.HankToZen(ptByomei.Byomei).Contains(HenkanJ.HankToZen(mst.ByomeiBui)))
                            {
                                ptByomeisContainByomeiBui.Add(ptByomei);
                                break;
                            }
                        }
                    }
                    foreach (var ptByomei in ptByomeisContainByomeiBui)
                    {
                        isValid = ValidateByomeiReflectOdrSite(compareName, HenkanJ.HankToZen(ptByomei.Byomei), buiOdrMst.LrKbn, buiOdrMst.BothKbn);
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
        return errorOdrInfDetails;
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

    public bool CheckDuplicateByomei(bool checkDuplicateByomei, bool checkDuplicateSyusyokuByomei, PtDiseaseModel currentPtByomeiModel, PtDiseaseModel comparedPtByomeiModel, int recehokenId)
    {
        if (!checkDuplicateByomei)
        {
            return false;
        }
        if (currentPtByomeiModel.IsFree || comparedPtByomeiModel.IsFree
           || (currentPtByomeiModel.HokenId != 0 && currentPtByomeiModel.HokenId != recehokenId)
           || (comparedPtByomeiModel.HokenId != 0 && comparedPtByomeiModel.HokenId != recehokenId)
           || currentPtByomeiModel.ByomeiCd != comparedPtByomeiModel.ByomeiCd)
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
            List<string> syusyokuCds = currentPtByomeiModel.PrefixSuffixList.Select(item => item.Code).ToList();
            List<string> compareSyusyokuCds = comparedPtByomeiModel.PrefixSuffixList.Select(item => item.Code).ToList();
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

    internal class BuiErrorModel
    {
        public OrdInfDetailModel OdrInfDetail { get; }
        public int OdrKouiKbn { get; }
        public int SinDate { get; }
        public string ItemName { get; }
        public List<string> Errors { get; }

        public BuiErrorModel(OrdInfDetailModel odrInfDetail, int odrKouiKbn, int sinDate, string itemName)
        {
            OdrInfDetail = odrInfDetail;
            OdrKouiKbn = odrKouiKbn;
            SinDate = sinDate;
            ItemName = itemName;
            Errors = new List<string>();
        }
    }
    #endregion

    #region Check Error action
    private List<ReceCheckErrModel> CheckHokenError(ReceRecalculationModel recalculationModel, List<ReceCheckErrModel> oldReceCheckErrList, List<ReceCheckErrModel> newReceCheckErrList, List<ReceCheckOptModel> receCheckOptList, List<SinKouiCountModel> sinKouiCountList)
    {
        //expired
        if (receCheckOptList.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.ExpiredEndDateHokenErrCd) && sinKouiCountList.Count > 0)
        {
            //hoken
            if (recalculationModel.HokenId > 0 && recalculationModel.HokenHoubetu.AsInteger() != 0)
            {
                //E1002 start date
                var firstSinKouiCount = sinKouiCountList.OrderBy(p => p.SinDate).FirstOrDefault(p => p.PtHokenPatterns.Any(q => q.HokenId == recalculationModel.HokenId));
                if (firstSinKouiCount != null && recalculationModel.HokenStartDate > 0 && recalculationModel.HokenStartDate > firstSinKouiCount.SinDate)
                {
                    AddReceCmtErrNew(oldReceCheckErrList, newReceCheckErrList, recalculationModel, ReceErrCdConst.ExpiredStartDateHokenErrCd,
                        ReceErrCdConst.ExpiredStartDateHokenErrMsg, "（" + CIUtil.SDateToShowSWDate(recalculationModel.HokenStartDate) + "～）", HOKEN_CHAR);
                }

                //E1001 end date
                var lastSinKouiCount = sinKouiCountList.OrderBy(p => p.SinDate).LastOrDefault(p => p.PtHokenPatterns.Any(q => q.HokenId == recalculationModel.HokenId));
                if (lastSinKouiCount != null && recalculationModel.HokenEndDate > 0 && recalculationModel.HokenEndDate < lastSinKouiCount.SinDate)
                {
                    AddReceCmtErrNew(oldReceCheckErrList, newReceCheckErrList, recalculationModel, ReceErrCdConst.ExpiredEndDateHokenErrCd,
                        ReceErrCdConst.ExpiredEndDateHokenErrMsg, "（～" + CIUtil.SDateToShowSWDate(recalculationModel.HokenEndDate) + "）", HOKEN_CHAR);
                }
            }
            //kohi1
            if (recalculationModel.Kohi1Id > 0 && recalculationModel.Kohi1Houbetu.AsInteger() != 102)
            {
                var firstSinKouiCount = sinKouiCountList.OrderBy(p => p.SinDate).FirstOrDefault(p => p.PtHokenPatterns
                    .Any(q => q.Kohi1Id == recalculationModel.Kohi1Id || q.Kohi2Id == recalculationModel.Kohi1Id || q.Kohi3Id == recalculationModel.Kohi1Id || q.Kohi4Id == recalculationModel.Kohi1Id));
                if (firstSinKouiCount != null && recalculationModel.Kohi1StartDate > 0 && recalculationModel.Kohi1StartDate > firstSinKouiCount.SinDate)
                {
                    AddReceCmtErrNew(oldReceCheckErrList, newReceCheckErrList, recalculationModel, ReceErrCdConst.ExpiredStartDateHokenErrCd,
                        ReceErrCdConst.ExpiredStartDateHokenErrMsg, "（" + CIUtil.SDateToShowSWDate(recalculationModel.Kohi1StartDate) + "～）", KOHI1_CHAR);
                }

                var lastSinKouiCount = sinKouiCountList.OrderBy(p => p.SinDate).LastOrDefault(p => p.PtHokenPatterns
                    .Any(q => q.Kohi1Id == recalculationModel.Kohi1Id || q.Kohi2Id == recalculationModel.Kohi1Id || q.Kohi3Id == recalculationModel.Kohi1Id || q.Kohi4Id == recalculationModel.Kohi1Id));
                if (lastSinKouiCount != null && recalculationModel.Kohi1EndDate > 0 && recalculationModel.Kohi1EndDate < lastSinKouiCount.SinDate)
                {
                    AddReceCmtErrNew(oldReceCheckErrList, newReceCheckErrList, recalculationModel, ReceErrCdConst.ExpiredEndDateHokenErrCd,
                        ReceErrCdConst.ExpiredEndDateHokenErrMsg, "（～" + CIUtil.SDateToShowSWDate(recalculationModel.Kohi1EndDate) + "）", KOHI1_CHAR);
                }
            }
            //kohi2
            if (recalculationModel.Kohi2Id > 0 && recalculationModel.Kohi2Houbetu.AsInteger() != 102)
            {
                var firstSinKouiCount = sinKouiCountList.OrderBy(p => p.SinDate).FirstOrDefault(p => p.PtHokenPatterns
                    .Any(q => q.Kohi1Id == recalculationModel.Kohi2Id || q.Kohi2Id == recalculationModel.Kohi2Id || q.Kohi3Id == recalculationModel.Kohi2Id || q.Kohi4Id == recalculationModel.Kohi2Id));
                if (firstSinKouiCount != null && recalculationModel.Kohi2StartDate > 0 && recalculationModel.Kohi2StartDate > firstSinKouiCount.SinDate)
                {
                    AddReceCmtErrNew(oldReceCheckErrList, newReceCheckErrList, recalculationModel, ReceErrCdConst.ExpiredStartDateHokenErrCd,
                        ReceErrCdConst.ExpiredStartDateHokenErrMsg, "（" + CIUtil.SDateToShowSWDate(recalculationModel.Kohi2StartDate) + "～）", KOHI2_CHAR);
                }

                var lastSinKouiCount = sinKouiCountList.OrderBy(p => p.SinDate).LastOrDefault(p => p.PtHokenPatterns
                    .Any(q => q.Kohi1Id == recalculationModel.Kohi2Id || q.Kohi2Id == recalculationModel.Kohi2Id || q.Kohi3Id == recalculationModel.Kohi2Id || q.Kohi4Id == recalculationModel.Kohi2Id));
                if (lastSinKouiCount != null && recalculationModel.Kohi2EndDate > 0 && recalculationModel.Kohi2EndDate < lastSinKouiCount.SinDate)
                {
                    AddReceCmtErrNew(oldReceCheckErrList, newReceCheckErrList, recalculationModel, ReceErrCdConst.ExpiredEndDateHokenErrCd,
                        ReceErrCdConst.ExpiredEndDateHokenErrMsg, "（～" + CIUtil.SDateToShowSWDate(recalculationModel.Kohi2EndDate) + "）", KOHI2_CHAR);
                }
            }
            //kohi3
            if (recalculationModel.Kohi3Id > 0 && recalculationModel.Kohi3Houbetu.AsInteger() != 102)
            {
                var firstSinKouiCount = sinKouiCountList.OrderBy(p => p.SinDate).FirstOrDefault(p => p.PtHokenPatterns
                    .Any(q => q.Kohi1Id == recalculationModel.Kohi3Id || q.Kohi2Id == recalculationModel.Kohi3Id || q.Kohi3Id == recalculationModel.Kohi3Id || q.Kohi4Id == recalculationModel.Kohi3Id));
                if (firstSinKouiCount != null && recalculationModel.Kohi3StartDate > 0 && recalculationModel.Kohi3StartDate > firstSinKouiCount.SinDate)
                {
                    AddReceCmtErrNew(oldReceCheckErrList, newReceCheckErrList, recalculationModel, ReceErrCdConst.ExpiredStartDateHokenErrCd,
                        ReceErrCdConst.ExpiredStartDateHokenErrMsg, "（" + CIUtil.SDateToShowSWDate(recalculationModel.Kohi3StartDate) + "～）", KOHI3_CHAR);
                }

                var lastSinKouiCount = sinKouiCountList.OrderBy(p => p.SinDate).LastOrDefault(p => p.PtHokenPatterns
                    .Any(q => q.Kohi1Id == recalculationModel.Kohi3Id || q.Kohi2Id == recalculationModel.Kohi3Id || q.Kohi3Id == recalculationModel.Kohi3Id || q.Kohi4Id == recalculationModel.Kohi3Id));
                if (lastSinKouiCount != null && recalculationModel.Kohi3EndDate > 0 && recalculationModel.Kohi3EndDate < lastSinKouiCount.SinDate)
                {
                    AddReceCmtErrNew(oldReceCheckErrList, newReceCheckErrList, recalculationModel, ReceErrCdConst.ExpiredEndDateHokenErrCd,
                        ReceErrCdConst.ExpiredEndDateHokenErrMsg, "（～" + CIUtil.SDateToShowSWDate(recalculationModel.Kohi3EndDate) + "）", KOHI3_CHAR);
                }
            }
            //kohi4
            if (recalculationModel.Kohi4Id > 0 && recalculationModel.Kohi4Houbetu.AsInteger() != 102)
            {
                var firstSinKouiCount = sinKouiCountList.OrderBy(p => p.SinDate).FirstOrDefault(p => p.PtHokenPatterns
                    .Any(q => q.Kohi1Id == recalculationModel.Kohi4Id || q.Kohi2Id == recalculationModel.Kohi4Id || q.Kohi3Id == recalculationModel.Kohi4Id || q.Kohi4Id == recalculationModel.Kohi4Id));
                if (firstSinKouiCount != null && recalculationModel.Kohi4StartDate > 0 && recalculationModel.Kohi4StartDate > firstSinKouiCount.SinDate)
                {
                    AddReceCmtErrNew(oldReceCheckErrList, newReceCheckErrList, recalculationModel, ReceErrCdConst.ExpiredStartDateHokenErrCd,
                        ReceErrCdConst.ExpiredStartDateHokenErrMsg, "（" + CIUtil.SDateToShowSWDate(recalculationModel.Kohi4StartDate) + "～）", KOHI4_CHAR);
                }

                var lastSinKouiCount = sinKouiCountList.OrderBy(p => p.SinDate).LastOrDefault(p => p.PtHokenPatterns
                    .Any(q => q.Kohi1Id == recalculationModel.Kohi4Id || q.Kohi2Id == recalculationModel.Kohi4Id || q.Kohi3Id == recalculationModel.Kohi4Id || q.Kohi4Id == recalculationModel.Kohi4Id));
                if (lastSinKouiCount != null && recalculationModel.Kohi4EndDate > 0 && recalculationModel.Kohi4EndDate < lastSinKouiCount.SinDate)
                {
                    AddReceCmtErrNew(oldReceCheckErrList, newReceCheckErrList, recalculationModel, ReceErrCdConst.ExpiredEndDateHokenErrCd,
                        ReceErrCdConst.ExpiredEndDateHokenErrMsg, "（～" + CIUtil.SDateToShowSWDate(recalculationModel.Kohi4EndDate) + "）", KOHI4_CHAR);
                }
            }
        }

        //E1003 unconfirmed
        if (receCheckOptList.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.UnConfirmedHokenErrCd))
        {
            if (recalculationModel.HokenId > 0 && !recalculationModel.IsHokenConfirmed)
            {
                string latestConfirmedDate = string.Empty;
                if (recalculationModel.LatestHokenConfirmedDate > 0)
                {
                    latestConfirmedDate = CIUtil.SDateToShowSWDate(recalculationModel.LatestHokenConfirmedDate);
                }
                AddReceCmtErrNew(oldReceCheckErrList, newReceCheckErrList, recalculationModel, ReceErrCdConst.UnConfirmedHokenErrCd,
                        ReceErrCdConst.UnConfirmedHokenErrMsg, "（最終確認: " + latestConfirmedDate + "）", HOKEN_CHAR);
            }
            if (recalculationModel.Kohi1Id > 0 && !recalculationModel.IsKohi1Confirmed)
            {
                string latestConfirmedDate = string.Empty;
                if (recalculationModel.LatestKohi1ConfirmedDate > 0)
                {
                    latestConfirmedDate = CIUtil.SDateToShowSWDate(recalculationModel.LatestKohi1ConfirmedDate);
                }
                AddReceCmtErrNew(oldReceCheckErrList, newReceCheckErrList, recalculationModel, ReceErrCdConst.UnConfirmedHokenErrCd,
                        ReceErrCdConst.UnConfirmedHokenErrMsg, "（最終確認: " + latestConfirmedDate + "）", KOHI1_CHAR);
            }
            if (recalculationModel.Kohi2Id > 0 && !recalculationModel.IsKohi2Confirmed)
            {
                string latestConfirmedDate = string.Empty;
                if (recalculationModel.LatestKohi2ConfirmedDate > 0)
                {
                    latestConfirmedDate = CIUtil.SDateToShowSWDate(recalculationModel.LatestKohi2ConfirmedDate);
                }
                AddReceCmtErrNew(oldReceCheckErrList, newReceCheckErrList, recalculationModel, ReceErrCdConst.UnConfirmedHokenErrCd,
                        ReceErrCdConst.UnConfirmedHokenErrMsg, "（最終確認: " + latestConfirmedDate + "）", KOHI2_CHAR);
            }
            if (recalculationModel.Kohi3Id > 0 && !recalculationModel.IsKohi3Confirmed)
            {
                string latestConfirmedDate = string.Empty;
                if (recalculationModel.LatestKohi3ConfirmedDate > 0)
                {
                    latestConfirmedDate = CIUtil.SDateToShowSWDate(recalculationModel.LatestKohi3ConfirmedDate);
                }
                AddReceCmtErrNew(oldReceCheckErrList, newReceCheckErrList, recalculationModel, ReceErrCdConst.UnConfirmedHokenErrCd,
                        ReceErrCdConst.UnConfirmedHokenErrMsg, "（最終確認: " + latestConfirmedDate + "）", KOHI3_CHAR);
            }
            if (recalculationModel.Kohi4Id > 0 && !recalculationModel.IsKohi4Confirmed)
            {
                string latestConfirmedDate = string.Empty;
                if (recalculationModel.LatestKohi4ConfirmedDate > 0)
                {
                    latestConfirmedDate = CIUtil.SDateToShowSWDate(recalculationModel.LatestKohi4ConfirmedDate);
                }
                AddReceCmtErrNew(oldReceCheckErrList, newReceCheckErrList, recalculationModel, ReceErrCdConst.UnConfirmedHokenErrCd,
                        ReceErrCdConst.UnConfirmedHokenErrMsg, "（最終確認: " + latestConfirmedDate + "）", KOHI4_CHAR);
            }
        }
        return newReceCheckErrList;
    }

    private List<ReceCheckErrModel> CheckByomeiError(int hpId, ReceRecalculationModel recalculationModel, List<ReceCheckErrModel> oldReceCheckErrList, List<ReceCheckErrModel> newReceCheckErrList, List<ReceCheckOptModel> receCheckOptList, List<SinKouiCountModel> sinKouiCountList, ref List<BuiErrorModel> errorOdrInfDetails)
    {
        bool visibleBuiOrderCheck = _systemConfRepository.GetSettingValue(6003, 0, hpId) == 1;
        var ptByomeis = _ptDiseaseRepository.GetByomeiInThisMonth(hpId, recalculationModel.SinYm, recalculationModel.PtId, recalculationModel.HokenId);
        if (ptByomeis.Count == 0)
        {
            //E2001 not exist byomei in month
            if (receCheckOptList.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.NotExistByomeiErrCd))
            {
                var sinKouiDetail = sinKouiCountList.FirstOrDefault(p => p.IsFirstVisit);
                if (sinKouiDetail != null)
                {
                    string msg2 = string.Format("（初診: {0}）", CIUtil.SDateToShowSWDate(sinKouiDetail.SinDate));
                    AddReceCmtErrNew(oldReceCheckErrList, newReceCheckErrList, recalculationModel, ReceErrCdConst.NotExistByomeiErrCd, ReceErrCdConst.NotExistByomeiErrMsg, msg2, sinDate: sinKouiDetail.SinDate);
                }
                else
                {
                    AddReceCmtErrNew(oldReceCheckErrList, newReceCheckErrList, recalculationModel, ReceErrCdConst.NotExistByomeiErrCd, ReceErrCdConst.NotExistByomeiErrMsg);
                }
            }

            //E2011 Bui Order Byomei
            if (visibleBuiOrderCheck && receCheckOptList.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.BuiOrderByomeiErrCd))
            {
                foreach (var sindate in sinKouiCountList.Select(item => item.SinDate).ToList())
                {
                    var odrInfs = _receiptRepository.GetOdrInfsBySinDate(hpId, recalculationModel.PtId, sindate, recalculationModel.HokenId);
                    var buiOdrItemMsts = _receiptRepository.GetBuiOdrItemMstList(hpId);
                    var buiOdrItemByomeiMsts = _receiptRepository.GetBuiOdrItemByomeiMstList(hpId);
                    List<string> msgErrors = CheckBuiOrderByomei(buiOdrItemMsts, buiOdrItemByomeiMsts, odrInfs, ptByomeis);
                    if (msgErrors.Count > 0)
                    {
                        foreach (var msgError in msgErrors)
                        {
                            string itemName = odrInfs.FirstOrDefault(p => p.ItemCd == msgError)?.ItemName ?? string.Empty;
                            AddReceCmtErrNew(oldReceCheckErrList, newReceCheckErrList, recalculationModel, ReceErrCdConst.BuiOrderByomeiErrCd,
                                                                       ReceErrCdConst.BuiOrderByomeiErrMsg,
                                                                       itemName + " : " +
                                                                       CIUtil.SDateToShowSWDate(sindate) + "）",
                                                                       msgError, sinDate: sindate);
                        }
                    }
                }
            }

            if (visibleBuiOrderCheck && receCheckOptList.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.ByomeiBuiOrderByomeiChekkuErrCd))
            {
                var odrInfModels = _ordInfRepository.GetList(hpId, recalculationModel.PtId, recalculationModel.SinYm, recalculationModel.HokenId);
                var buiOdrMsts = _receiptRepository.GetBuiOdrMstList(hpId);
                var buiOdrByomeiMsts = _receiptRepository.GetBuiOdrByomeiMstList(hpId);
                errorOdrInfDetails = CheckByomeiWithBuiOdr(errorOdrInfDetails, odrInfModels, buiOdrMsts, buiOdrByomeiMsts, ptByomeis);
                foreach (var errorOdrInfDetail in errorOdrInfDetails)
                {
                    foreach (var msg in errorOdrInfDetail.Errors)
                    {
                        AddReceCmtErrNew(oldReceCheckErrList, newReceCheckErrList, recalculationModel, ReceErrCdConst.ByomeiBuiOrderByomeiChekkuErrCd, ReceErrCdConst.ByomeiBuiOrderByomeiChekkuErrMsg, msg, aCd: errorOdrInfDetail.ItemName, bCd: errorOdrInfDetail.OdrKouiKbn.ToString(), sinDate: errorOdrInfDetail.SinDate);
                    }
                }
            }
        }
        else
        {
            //check if exist continous byomei in first visit or revisit
            if (receCheckOptList.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.NotExistByomeiErrCd) || receCheckOptList.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.CheckFirstVisit2003ByomeiErrCd))
            {
                string format = string.Empty;
                string msg2 = string.Empty;
                foreach (var sinKouiDetail in sinKouiCountList)
                {
                    //E2002 revisit
                    if (receCheckOptList.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.NotExistByomeiErrCd) &&
                        sinKouiDetail.IsReVisit && !ptByomeis.Any(p => p.StartDate <= sinKouiDetail.SinDate &&
                        (p.TenkiDate >= sinKouiDetail.SinDate || p.IsContinous)))
                    {
                        format = "（再診: {0}）";
                        msg2 = string.Format(format, CIUtil.SDateToShowSWDate(sinKouiDetail.SinDate));
                        AddReceCmtErrNew(oldReceCheckErrList, newReceCheckErrList, recalculationModel, ReceErrCdConst.CheckReVisitContiByomeiErrCd, ReceErrCdConst.CheckReVisitContiByomeiErrMsg, msg2, sinDate: sinKouiDetail.SinDate);
                    }
                    //first visit
                    else if (receCheckOptList.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.CheckFirstVisit2003ByomeiErrCd) && sinKouiDetail.IsFirstVisit)
                    {
                        //E2004
                        List<PtDiseaseModel> checkedPtByomeis = new();
                        if (!sinKouiCountList.Any(p => p.SinDate == sinKouiDetail.SinDate && p.ExistSameFirstVisit))
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
                                    AddReceCmtErrNew(oldReceCheckErrList, newReceCheckErrList, recalculationModel, ReceErrCdConst.CheckFirstVisit2003ByomeiErrCd, ReceErrCdConst.CheckFirstVisit2004ByomeiErrMsg,
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
                            AddReceCmtErrNew(oldReceCheckErrList, newReceCheckErrList, recalculationModel, ReceErrCdConst.CheckFirstVisit2003ByomeiErrCd, ReceErrCdConst.CheckFirstVisit2003ByomeiErrMsg, msg2, sinDate: sinKouiDetail.SinDate);
                        }
                    }
                }
            }

            //E2005 check if has not main byomei 
            if (receCheckOptList.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.HasNotMainByomeiErrCd) && !ptByomeis.Any(p => p.IsMain))
            {
                AddReceCmtErrNew(oldReceCheckErrList, newReceCheckErrList, recalculationModel, ReceErrCdConst.HasNotMainByomeiErrCd, ReceErrCdConst.HasNotMainByomeiErrMsg);
            }

            //E2006 check abandonment byomei
            if (receCheckOptList.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.InvalidByomeiErrCd))
            {
                foreach (var ptByomei in ptByomeis)
                {
                    if (!ptByomei.IsFree && ptByomei.DelDate > 0 && ptByomei.DelDate < recalculationModel.FirstDateOfThisMonth &&
                        (ptByomei.TenkiDate > ptByomei.DelDate || ptByomei.IsContinous))
                    {
                        string format = "（{0}: ～{1}）";
                        string msg2 = string.Format(format, ptByomei.Byomei, CIUtil.SDateToShowSWDate(ptByomei.DelDate));
                        AddReceCmtErrNew(oldReceCheckErrList, newReceCheckErrList, recalculationModel, ReceErrCdConst.InvalidByomeiErrCd, ReceErrCdConst.InvalidByomeiErrMsg,
                                                                   msg2, ptByomei.ByomeiCd);
                    }
                }
            }

            //E2007 check free text char count > 20
            if (receCheckOptList.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.FreeTextLengthByomeiErrCd))
            {
                foreach (var ptByomei in ptByomeis)
                {
                    if (ptByomei.IsFree && ptByomei.Byomei.Length > 20)
                    {
                        string cutByomei = CIUtil.Copy(ptByomei.Byomei, 1, 100);
                        string msg2 = string.Format("({0}: {1}/20文字)", cutByomei, ptByomei.Byomei.Length);
                        AddReceCmtErrNew(oldReceCheckErrList, newReceCheckErrList, recalculationModel, ReceErrCdConst.FreeTextLengthByomeiErrCd, ReceErrCdConst.FreeTextLengthByomeiErrMsg, msg2, cutByomei);
                    }
                }
            }

            //E2008 check suspected byomei
            var receCheckOpt = receCheckOptList.FirstOrDefault(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.CheckSuspectedByomeiErrCd);
            if (receCheckOpt != null)
            {
                foreach (var ptByomei in ptByomeis)
                {
                    if (ptByomei.Byomei.AsString().Contains(SUSPECTED_SUFFIX) &&
                        CIUtil.DateTimeToInt(CIUtil.IntToDate(ptByomei.StartDate).AddMonths(receCheckOpt.CheckOpt)) <= recalculationModel.LastDateOfThisMonth)
                    {
                        string format = "（{0}: {1}～）";
                        string cutByomei = CIUtil.Copy(ptByomei.Byomei, 1, 100);
                        string msg2 = string.Format(format, cutByomei, CIUtil.SDateToShowSWDate(ptByomei.StartDate));
                        AddReceCmtErrNew(oldReceCheckErrList, newReceCheckErrList, recalculationModel, ReceErrCdConst.CheckSuspectedByomeiErrCd,
                            ReceErrCdConst.CheckSuspectedByomeiErrMsg.Replace("xx", receCheckOpt.CheckOpt.AsString()), msg2, cutByomei);
                    }
                }
            }

            bool checkDuplicateByomei = receCheckOptList.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.DuplicateByomeiCheckErrCd);
            bool checkDuplicateSyusyokuByomei = receCheckOptList.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.DuplicateSyusyokuByomeiCheckErrCd);
            if (checkDuplicateByomei)
            {
                List<PtDiseaseModel> checkedByomeiList = new List<PtDiseaseModel>();
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
                        bool isDuplicate = CheckDuplicateByomei(checkDuplicateByomei, checkDuplicateSyusyokuByomei, ptByomei, comparedPtByomei, recalculationModel.HokenId);
                        if (isDuplicate)
                        {
                            checkedByomeiList.Add(ptByomei);
                            break;
                        }
                    }
                }
                foreach (var ptByomei in checkedByomeiList)
                {
                    AddReceCmtErrNew(oldReceCheckErrList, newReceCheckErrList, recalculationModel, ReceErrCdConst.DuplicateByomeiCheckErrCd,
                                                                          ReceErrCdConst.DuplicateByomeiCheckErrMsg,
                                                                          "（" + ptByomei.Byomei + " : " + CIUtil.SDateToShowSWDate(ptByomei.StartDate) + "）",
                                                                          ptByomei.ByomeiCd, string.Join(string.Empty, ptByomei.PrefixSuffixList.Select(item => item.Code).ToArray()));
                }
            }

            bool checkByomeiResponding = receCheckOptList.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.HasNotByomeiWithOdrErrCd);
            bool checkBuiOrderByomei = receCheckOptList.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.BuiOrderByomeiErrCd);
            if (checkByomeiResponding || checkBuiOrderByomei)
            {
                foreach (var sindate in sinKouiCountList.Select(item => item.SinDate).ToList())
                {
                    var odrInfs = _receiptRepository.GetOdrInfsBySinDate(hpId, recalculationModel.PtId, sindate, recalculationModel.HokenId);

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

                            string santeiItemCd = _receiptRepository.GetSanteiItemCd(hpId, itemCd, sindate);

                            List<string> tekiouByomeiCds = _receiptRepository.GetTekiouByomei(hpId, new List<string>() { itemCd, santeiItemCd });
                            if (tekiouByomeiCds.Count == 0) continue;

                            if (!ptByomeis.Where(p => p.StartDate <= odrInf.SinDate && (!odrInf.IsDrug || !p.Byomei.AsString().Contains(SUSPECTED_SUFFIX)))
                                         .Any(p => tekiouByomeiCds.Contains(p.ByomeiCd)))
                            {
                                checkedItemCds.Add(odrInf.ItemCd);
                                if (checkedItemCds.Count(p => p == odrInf.ItemCd) == 1)
                                {
                                    AddReceCmtErrNew(oldReceCheckErrList, newReceCheckErrList, recalculationModel, ReceErrCdConst.HasNotByomeiWithOdrErrCd,
                                                                               ReceErrCdConst.HasNotByomeiWithOdrErrMsg,
                                                                               "（" + odrInf.ItemName + " : " +
                                                                               CIUtil.SDateToShowSWDate(sindate) + "）",
                                                                               odrInf.ItemCd);
                                }
                            }
                        }
                    }


                    //E2011 check bui order byomei
                    if (visibleBuiOrderCheck && checkBuiOrderByomei)
                    {
                        var buiOdrItemMsts = _receiptRepository.GetBuiOdrItemMstList(hpId);
                        var buiOdrItemByomeiMsts = _receiptRepository.GetBuiOdrItemByomeiMstList(hpId);
                        List<string> msgErrors = CheckBuiOrderByomei(buiOdrItemMsts, buiOdrItemByomeiMsts, odrInfs, ptByomeis);
                        if (msgErrors.Count > 0)
                        {
                            foreach (var msgError in msgErrors)
                            {
                                string itemName = odrInfs.FirstOrDefault(p => p.ItemCd == msgError)?.ItemName ?? string.Empty;
                                AddReceCmtErrNew(oldReceCheckErrList, newReceCheckErrList, recalculationModel, ReceErrCdConst.BuiOrderByomeiErrCd,
                                                                           ReceErrCdConst.BuiOrderByomeiErrMsg,
                                                                           itemName + " : " +
                                                                           CIUtil.SDateToShowSWDate(sindate) + "）",
                                                                           msgError, sinDate: sindate);
                            }
                        }
                    }
                }
            }

            if (visibleBuiOrderCheck && receCheckOptList.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.ByomeiBuiOrderByomeiChekkuErrCd))
            {
                foreach (var errorOdrInfDetail in errorOdrInfDetails)
                {
                    foreach (var msg in errorOdrInfDetail.Errors)
                    {
                        AddReceCmtErrNew(oldReceCheckErrList, newReceCheckErrList, recalculationModel, ReceErrCdConst.ByomeiBuiOrderByomeiChekkuErrCd, ReceErrCdConst.ByomeiBuiOrderByomeiChekkuErrMsg, msg, aCd: errorOdrInfDetail.ItemName, bCd: errorOdrInfDetail.OdrKouiKbn.ToString(), sinDate: errorOdrInfDetail.SinDate);
                    }
                }
            }
        }
        return newReceCheckErrList;
    }
    #endregion
}
