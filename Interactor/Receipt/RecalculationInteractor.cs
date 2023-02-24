﻿using Domain.Models.Diseases;
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
    private const string _hokenChar = "0";
    private const string _kohi1Char = "1";
    private const string _kohi2Char = "2";
    private const string _kohi3Char = "3";
    private const string _kohi4Char = "4";
    private const string _freeWord = "0000999";
    private const string _suspectedSuffix = "の疑い";
    private const string _left = "左";
    private const string _right = "右";
    private const string _both = "両";
    private const string _leftRight = "左右";
    private const string _rightLeft = "右左";

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
        foreach (var itemCd in todayOrderInfModels.Select(item => item.ItemCd).ToList())
        {
            if (!buiOdrItemMsts.Any(p => p.ItemCd == itemCd)) continue;

            var buiOdrByomeiMsts = buiOdrItemByomeiMsts.Where(p => p.ItemCd == itemCd).ToList();
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
                        if (PtDiseaseModels.Any(p => (p.ByomeiHankToZen.ToString().Contains(_left) || p.ByomeiHankToZen.ToString().Contains(_right) ||
                            p.ByomeiHankToZen.ToString().Contains(_both)) && buiOdrByomeiMsts.Any(q => p.ByomeiHankToZen.Contains(HenkanJ.HankToZen(q.ByomeiBui)))))
                        {
                            hasError = false;
                            break;
                        }
                    }
                    else if (buiOdrByomeiMst.LrKbn == 1 && buiOdrByomeiMst.BothKbn == 0)
                    {
                        if (PtDiseaseModels.Any(p => (p.ByomeiHankToZen.ToString().Contains(_left) || p.ByomeiHankToZen.ToString().Contains(_right))
                            && !p.ByomeiHankToZen.ToString().Contains(_leftRight) && !p.ByomeiHankToZen.ToString().Contains(_rightLeft) && buiOdrByomeiMsts.Any(q => p.Byomei.Contains(HenkanJ.HankToZen(q.ByomeiBui)))))
                        {
                            hasError = false;
                            break;
                        }
                    }
                    else if (buiOdrByomeiMst.LrKbn == 0 && buiOdrByomeiMst.BothKbn == 1 && PtDiseaseModels.Any(p => (p.ByomeiHankToZen.ToString().Contains(_both) || p.ByomeiHankToZen.ToString().Contains(_leftRight) || p.ByomeiHankToZen.ToString().Contains(_rightLeft))
                            && buiOdrByomeiMsts.Any(q => p.Byomei.Contains(HenkanJ.HankToZen(q.ByomeiBui)))))
                    {
                        hasError = false;
                        break;
                    }
                }
                if (hasError)
                {
                    msgErrors.Add(itemCd);
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
                            odrBuiPatterns.Add($"{_both}{buiOdrMst.OdrBui}");
                            odrBuiPatterns.Add($"{_left}{buiOdrMst.OdrBui}");
                            odrBuiPatterns.Add($"{_right}{buiOdrMst.OdrBui}");
                            odrBuiPatterns.Add($"{_leftRight}{buiOdrMst.OdrBui}");
                            odrBuiPatterns.Add($"{_rightLeft}{buiOdrMst.OdrBui}");
                        }
                        else if (buiOdrMst.LrKbn == 1 && buiOdrMst.BothKbn == 0)
                        {
                            odrBuiPatterns.Add($"{_left}{buiOdrMst.OdrBui}");
                            odrBuiPatterns.Add($"{_right}{buiOdrMst.OdrBui}");
                        }
                        else if (buiOdrMst.LrKbn == 0 && buiOdrMst.BothKbn == 1)
                        {
                            odrBuiPatterns.Add($"{_both}{buiOdrMst.OdrBui}");
                            odrBuiPatterns.Add($"{_leftRight}{buiOdrMst.OdrBui}");
                            odrBuiPatterns.Add($"{_rightLeft}{buiOdrMst.OdrBui}");
                        }
                    }
                    else
                    {
                        odrBuiPatterns.Add(buiOdrMst.OdrBui);
                    }
                    var ptByomeiAdd = odrBuiPatterns.FirstOrDefault(pattern => compareName.Contains(HenkanJ.HankToZen(pattern)));
                    if (ptByomeiAdd != null)
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
                    var filteredBuiOdrByomeiMsts = buiOdrByomeiMstList.Where(mst => mst.BuiId == buiOdrMst.BuiId);
                    List<PtDiseaseModel> ptByomeisContainByomeiBui = new();
                    foreach (var ptByomei in ptByomeiList)
                    {
                        var ptByomeiAdd = filteredBuiOdrByomeiMsts.FirstOrDefault(mst => HenkanJ.HankToZen(ptByomei.Byomei).Contains(HenkanJ.HankToZen(mst.ByomeiBui)));
                        if (ptByomeiAdd != null)
                        {
                            ptByomeisContainByomeiBui.Add(ptByomei);
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
            if (str.Contains(_both))
            {
                return _both;
            }
            else if (str == $"{_left}{_right}" || str == $"{_right}{_left}")
            {
                return str;
            }
            else if (str.Contains(_left))
            {
                return _left;
            }
            else if (str.Contains(_right))
            {
                return _right;
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
            string buiOdrLeftRight = buiOdrDirection.Replace($"{_both}", $"{_left}{_right}").Replace($"{_right}{_left}", $"{_left}{_right}");
            string byomeiNameLeftRight = byomeiNameDirection.Replace($"{_both}", $"{_left}{_right}").Replace($"{_right}{_left}", $"{_left}{_right}");
            return byomeiNameLeftRight.Contains(buiOdrLeftRight);
        }
        else if (LrKbn == 1 && BothKbn == 0)
        {
            string buiOdrDirection = GetDirection(buiOdr);
            string byomeiNameDirection = GetDirection(byomeiName);

            // Convert names to the left-right direction if they contain 両 character or right-left direction.
            string buiOdrLeftRight = buiOdrDirection.Replace($"{_both}", $"{_left}{_right}").Replace($"{_right}{_left}", $"{_left}{_right}");
            string byomeiNameLeftRight = byomeiNameDirection.Replace($"{_both}", $"{_left}{_right}").Replace($"{_right}{_left}", $"{_left}{_right}");
            if (byomeiNameLeftRight.Contains($"{_left}{_right}") && (buiOdrLeftRight == _left || buiOdrLeftRight == _right))
            {
                return false;
            }
            return byomeiNameLeftRight.Contains(buiOdrLeftRight);
        }
        return true;
    }

    private bool CheckDuplicateByomei(bool checkDuplicateByomei, bool checkDuplicateSyusyokuByomei, PtDiseaseModel currentPtByomeiModel, PtDiseaseModel comparedPtByomeiModel, int recehokenId)
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

    private List<DayLimitResultModel> CheckOnlyDayLimitOrder(OrdInfModel todayOdrInfModel, long ptId, int sinDate)
    {
        RealtimeChecker<TodayOdrInfModel, TodayOdrInfDetailModel> realtimeChecker = new RealtimeChecker<TodayOdrInfModel, TodayOdrInfDetailModel>();
        realtimeChecker.InjectProperties(Session.HospitalID, ptId, sinDate);
        realtimeChecker.InjectFinder(_realtimeCheckerFinder, _masterFinder);
        return CheckOnlyDayLimit(todayOdrInfModel);
    }

    public List<DayLimitResultModel> CheckOnlyDayLimit(TOdrInf checkingOrder)
    {
        UnitChecker<TOdrInf, TOdrDetail> dayLimitChecker =
            new DayLimitChecker<TOdrInf, TOdrDetail>()
            {
                CheckType = RealtimeCheckerType.Days
            };
        InitUnitCheck(dayLimitChecker);

        UnitCheckerForOrderListResult<TOdrInf, TOdrDetail> checkedResult = dayLimitChecker.CheckOrderList(new List<TOdrInf>() { checkingOrder });
        List<DayLimitResultModel> result = checkedResult.ErrorInfo as List<DayLimitResultModel>;
        return result ?? new List<DayLimitResultModel>();
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
                        ReceErrCdConst.ExpiredStartDateHokenErrMsg, "（" + CIUtil.SDateToShowSWDate(recalculationModel.HokenStartDate) + "～）", _hokenChar);
                }

                //E1001 end date
                var lastSinKouiCount = sinKouiCountList.OrderBy(p => p.SinDate).LastOrDefault(p => p.PtHokenPatterns.Any(q => q.HokenId == recalculationModel.HokenId));
                if (lastSinKouiCount != null && recalculationModel.HokenEndDate > 0 && recalculationModel.HokenEndDate < lastSinKouiCount.SinDate)
                {
                    AddReceCmtErrNew(oldReceCheckErrList, newReceCheckErrList, recalculationModel, ReceErrCdConst.ExpiredEndDateHokenErrCd,
                        ReceErrCdConst.ExpiredEndDateHokenErrMsg, "（～" + CIUtil.SDateToShowSWDate(recalculationModel.HokenEndDate) + "）", _hokenChar);
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
                        ReceErrCdConst.ExpiredStartDateHokenErrMsg, "（" + CIUtil.SDateToShowSWDate(recalculationModel.Kohi1StartDate) + "～）", _kohi1Char);
                }

                var lastSinKouiCount = sinKouiCountList.OrderBy(p => p.SinDate).LastOrDefault(p => p.PtHokenPatterns
                    .Any(q => q.Kohi1Id == recalculationModel.Kohi1Id || q.Kohi2Id == recalculationModel.Kohi1Id || q.Kohi3Id == recalculationModel.Kohi1Id || q.Kohi4Id == recalculationModel.Kohi1Id));
                if (lastSinKouiCount != null && recalculationModel.Kohi1EndDate > 0 && recalculationModel.Kohi1EndDate < lastSinKouiCount.SinDate)
                {
                    AddReceCmtErrNew(oldReceCheckErrList, newReceCheckErrList, recalculationModel, ReceErrCdConst.ExpiredEndDateHokenErrCd,
                        ReceErrCdConst.ExpiredEndDateHokenErrMsg, "（～" + CIUtil.SDateToShowSWDate(recalculationModel.Kohi1EndDate) + "）", _kohi1Char);
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
                        ReceErrCdConst.ExpiredStartDateHokenErrMsg, "（" + CIUtil.SDateToShowSWDate(recalculationModel.Kohi2StartDate) + "～）", _kohi2Char);
                }

                var lastSinKouiCount = sinKouiCountList.OrderBy(p => p.SinDate).LastOrDefault(p => p.PtHokenPatterns
                    .Any(q => q.Kohi1Id == recalculationModel.Kohi2Id || q.Kohi2Id == recalculationModel.Kohi2Id || q.Kohi3Id == recalculationModel.Kohi2Id || q.Kohi4Id == recalculationModel.Kohi2Id));
                if (lastSinKouiCount != null && recalculationModel.Kohi2EndDate > 0 && recalculationModel.Kohi2EndDate < lastSinKouiCount.SinDate)
                {
                    AddReceCmtErrNew(oldReceCheckErrList, newReceCheckErrList, recalculationModel, ReceErrCdConst.ExpiredEndDateHokenErrCd,
                        ReceErrCdConst.ExpiredEndDateHokenErrMsg, "（～" + CIUtil.SDateToShowSWDate(recalculationModel.Kohi2EndDate) + "）", _kohi2Char);
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
                        ReceErrCdConst.ExpiredStartDateHokenErrMsg, "（" + CIUtil.SDateToShowSWDate(recalculationModel.Kohi3StartDate) + "～）", _kohi3Char);
                }

                var lastSinKouiCount = sinKouiCountList.OrderBy(p => p.SinDate).LastOrDefault(p => p.PtHokenPatterns
                    .Any(q => q.Kohi1Id == recalculationModel.Kohi3Id || q.Kohi2Id == recalculationModel.Kohi3Id || q.Kohi3Id == recalculationModel.Kohi3Id || q.Kohi4Id == recalculationModel.Kohi3Id));
                if (lastSinKouiCount != null && recalculationModel.Kohi3EndDate > 0 && recalculationModel.Kohi3EndDate < lastSinKouiCount.SinDate)
                {
                    AddReceCmtErrNew(oldReceCheckErrList, newReceCheckErrList, recalculationModel, ReceErrCdConst.ExpiredEndDateHokenErrCd,
                        ReceErrCdConst.ExpiredEndDateHokenErrMsg, "（～" + CIUtil.SDateToShowSWDate(recalculationModel.Kohi3EndDate) + "）", _kohi3Char);
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
                        ReceErrCdConst.ExpiredStartDateHokenErrMsg, "（" + CIUtil.SDateToShowSWDate(recalculationModel.Kohi4StartDate) + "～）", _kohi4Char);
                }

                var lastSinKouiCount = sinKouiCountList.OrderBy(p => p.SinDate).LastOrDefault(p => p.PtHokenPatterns
                    .Any(q => q.Kohi1Id == recalculationModel.Kohi4Id || q.Kohi2Id == recalculationModel.Kohi4Id || q.Kohi3Id == recalculationModel.Kohi4Id || q.Kohi4Id == recalculationModel.Kohi4Id));
                if (lastSinKouiCount != null && recalculationModel.Kohi4EndDate > 0 && recalculationModel.Kohi4EndDate < lastSinKouiCount.SinDate)
                {
                    AddReceCmtErrNew(oldReceCheckErrList, newReceCheckErrList, recalculationModel, ReceErrCdConst.ExpiredEndDateHokenErrCd,
                        ReceErrCdConst.ExpiredEndDateHokenErrMsg, "（～" + CIUtil.SDateToShowSWDate(recalculationModel.Kohi4EndDate) + "）", _kohi4Char);
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
                        ReceErrCdConst.UnConfirmedHokenErrMsg, "（最終確認: " + latestConfirmedDate + "）", _hokenChar);
            }
            if (recalculationModel.Kohi1Id > 0 && !recalculationModel.IsKohi1Confirmed)
            {
                string latestConfirmedDate = string.Empty;
                if (recalculationModel.LatestKohi1ConfirmedDate > 0)
                {
                    latestConfirmedDate = CIUtil.SDateToShowSWDate(recalculationModel.LatestKohi1ConfirmedDate);
                }
                AddReceCmtErrNew(oldReceCheckErrList, newReceCheckErrList, recalculationModel, ReceErrCdConst.UnConfirmedHokenErrCd,
                        ReceErrCdConst.UnConfirmedHokenErrMsg, "（最終確認: " + latestConfirmedDate + "）", _kohi1Char);
            }
            if (recalculationModel.Kohi2Id > 0 && !recalculationModel.IsKohi2Confirmed)
            {
                string latestConfirmedDate = string.Empty;
                if (recalculationModel.LatestKohi2ConfirmedDate > 0)
                {
                    latestConfirmedDate = CIUtil.SDateToShowSWDate(recalculationModel.LatestKohi2ConfirmedDate);
                }
                AddReceCmtErrNew(oldReceCheckErrList, newReceCheckErrList, recalculationModel, ReceErrCdConst.UnConfirmedHokenErrCd,
                        ReceErrCdConst.UnConfirmedHokenErrMsg, "（最終確認: " + latestConfirmedDate + "）", _kohi2Char);
            }
            if (recalculationModel.Kohi3Id > 0 && !recalculationModel.IsKohi3Confirmed)
            {
                string latestConfirmedDate = string.Empty;
                if (recalculationModel.LatestKohi3ConfirmedDate > 0)
                {
                    latestConfirmedDate = CIUtil.SDateToShowSWDate(recalculationModel.LatestKohi3ConfirmedDate);
                }
                AddReceCmtErrNew(oldReceCheckErrList, newReceCheckErrList, recalculationModel, ReceErrCdConst.UnConfirmedHokenErrCd,
                        ReceErrCdConst.UnConfirmedHokenErrMsg, "（最終確認: " + latestConfirmedDate + "）", _kohi3Char);
            }
            if (recalculationModel.Kohi4Id > 0 && !recalculationModel.IsKohi4Confirmed)
            {
                string latestConfirmedDate = string.Empty;
                if (recalculationModel.LatestKohi4ConfirmedDate > 0)
                {
                    latestConfirmedDate = CIUtil.SDateToShowSWDate(recalculationModel.LatestKohi4ConfirmedDate);
                }
                AddReceCmtErrNew(oldReceCheckErrList, newReceCheckErrList, recalculationModel, ReceErrCdConst.UnConfirmedHokenErrCd,
                        ReceErrCdConst.UnConfirmedHokenErrMsg, "（最終確認: " + latestConfirmedDate + "）", _kohi4Char);
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
                    if (ptByomei.Byomei.ToString().Contains(_suspectedSuffix) &&
                        CIUtil.DateTimeToInt(CIUtil.IntToDate(ptByomei.StartDate).AddMonths(receCheckOpt.CheckOpt)) <= recalculationModel.LastDateOfThisMonth)
                    {
                        string format = "（{0}: {1}～）";
                        string cutByomei = CIUtil.Copy(ptByomei.Byomei, 1, 100);
                        string msg2 = string.Format(format, cutByomei, CIUtil.SDateToShowSWDate(ptByomei.StartDate));
                        AddReceCmtErrNew(oldReceCheckErrList, newReceCheckErrList, recalculationModel, ReceErrCdConst.CheckSuspectedByomeiErrCd,
                            ReceErrCdConst.CheckSuspectedByomeiErrMsg.Replace("xx", receCheckOpt.CheckOpt.ToString()), msg2, cutByomei);
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
                                itemCd == ItemCdConst.Con_Refill) { continue; }

                            string santeiItemCd = _receiptRepository.GetSanteiItemCd(hpId, itemCd, sindate);

                            List<string> tekiouByomeiCds = _receiptRepository.GetTekiouByomei(hpId, new List<string>() { itemCd, santeiItemCd });
                            if (tekiouByomeiCds.Count == 0) { continue; }

                            if (!ptByomeis.Where(p => p.StartDate <= odrInf.SinDate && (!odrInf.IsDrug || !p.Byomei.ToString().Contains(_suspectedSuffix)))
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

    private List<ReceCheckErrModel> CheckOrderError(int hpId, ReceRecalculationModel receInfModel, List<ReceCheckErrModel> oldReceCheckErrList, List<ReceCheckErrModel> newReceCheckErrList, List<ReceCheckOptModel> receCheckOptList, List<SinKouiCountModel> sinKouiCountList)
    {
        bool isCheckExceedDosage = receCheckOptList.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.ExceededDosageOdrErrCd);
        bool isCheckDuplicateOdr = receCheckOptList.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.DuplicateOdrErrCd);
        bool isCheckExpiredOdr = receCheckOptList.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.ExpiredEndDateOdrErrCd);
        bool isCheckFirstExamFee = receCheckOptList.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.FirstExamFeeCheckErrCd);
        bool isCheckSanteiCount = receCheckOptList.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.SanteiCountCheckErrCd);
        bool isCheckTokuzaiItem = receCheckOptList.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.TokuzaiItemCheckErrCd);
        bool isCheckItemAge = receCheckOptList.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.ItemAgeCheckErrCd);
        bool isCheckComment = receCheckOptList.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.CommentCheckErrCd);
        bool isCheckAdditionItem = receCheckOptList.Any(p => p.IsInvalid == 0 && p.ErrCd == ReceErrCdConst.AdditionItemErrCd);

        var odrInfModels = _ordInfRepository.GetList(hpId, receInfModel.PtId, receInfModel.SinYm, receInfModel.HokenId);
        List<OrdInfDetailModel> odrInfDetailModels = new();

        //OrderInf
        foreach (var odrInfModel in odrInfModels)
        {
            //E4001 check exceeded dosage
            if (isCheckExceedDosage)
            {
                var resultOdrs = CheckOnlyDayLimitOrder(odrInfModel, odrInfModel.PtId, odrInfModel.SinDate);
                foreach (var odr in resultOdrs)
                {
                    string msg2 = string.Format("（{0}: {1} [{2}日/{3}日]）", odr.ItemName, CIUtil.SDateToShowSWDate(todayOdrInf.SinDate), odr.UsingDay, odr.LimitDay);
                    _commandHandler.InsertReceCmtErr(receInfModel, ReceErrCdConst.ExceededDosageOdrErrCd, ReceErrCdConst.ExceededDosageOdrErrMsg,
                                                    msg2, odr.ItemCd, sinDate: todayOdrInf.SinDate);
                }
            }
            odrInfDetailModels.AddRange(odrInfModel.OdrInfDetailModels);
        }

        #region Duplicate check
        if (isCheckDuplicateOdr)
        {
            List<string> checkedOdrItemCds = new();
            foreach (var odrDetail in odrInfDetailModels)
            {
                int sinDate = odrDetail.SinDate;
                int endDate = odrDetail.SinDate;
                int syosinDate = _masterFinder.GetFirstVisitWithSyosin(receInfModel.PtId, sinDate);
                //E4002 check order with same effect
                if (isCheckDuplicateOdr)
                {
                    if (odrDetail.IsDrugOrInjection && !string.IsNullOrEmpty(odrDetail.YJCode))
                    {
                        var duplicatedOdr = odrInfDetailModels.FirstOrDefault(p => CIUtil.Copy(p.YJCode, 1, 4) == CIUtil.Copy(odrDetail.YJCode, 1, 4) &&
                                                                                   p.SinDate == odrDetail.SinDate &&
                                                                                   p.RaiinNo == odrDetail.RaiinNo &&
                                                                                   p.ItemCd != odrDetail.ItemCd);
                        if (duplicatedOdr != null)
                        {
                            if (!checkedOdrItemCds.Contains(odrDetail.ItemCd) || !checkedOdrItemCds.Contains(duplicatedOdr.ItemCd))
                            {
                                string msg2 = string.Format("（{0} : {1} [{2}]）", odrDetail.ItemName, duplicatedOdr.ItemName, CIUtil.SDateToShowSWDate(odrDetail.SinDate));
                                _commandHandler.InsertReceCmtErr(receInfModel, ReceErrCdConst.DuplicateOdrErrCd, ReceErrCdConst.DuplicateOdrErrMsg,
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
            foreach (var sinKouiCount in sinKouiCountList)
            {
                foreach (var sinKouiDetailModel in sinKouiCount.SinKouiDetailModels)
                {
                    string itemCd = sinKouiDetailModel.ItemCd;
                    if (!string.IsNullOrEmpty(itemCd) && sinKouiDetailModel.TenMst != null && !checkedItemCds.Contains(itemCd))
                    {
                        var lastTenMst = _masterFinder.FindLastTenMst(itemCd);
                        if (lastTenMst != null && sinKouiCount.SinDate > lastTenMst.EndDate)
                        {
                            string msg2 = string.Format("（{0} {1}: ～{2}）", sinKouiDetailModel.ItemName, CIUtil.SDateToShowSWDate(sinKouiCount.SinDate), CIUtil.SDateToShowSWDate(lastTenMst.EndDate));
                            _commandHandler.InsertReceCmtErr(receInfModel, ReceErrCdConst.ExpiredEndDateOdrErrCd, ReceErrCdConst.ExpiredEndDateOdrErrMsg, msg2, itemCd, sinDate: sinKouiCount.SinDate);
                        }

                        var firstTenMst = _masterFinder.FindFirstTenMst(itemCd);
                        if (firstTenMst != null && sinKouiCount.SinDate < firstTenMst.StartDate)
                        {
                            string msg2 = string.Format("（{0} {1}: {2}～）", sinKouiDetailModel.ItemName, CIUtil.SDateToShowSWDate(sinKouiCount.SinDate), CIUtil.SDateToShowSWDate(firstTenMst.StartDate));
                            _commandHandler.InsertReceCmtErr(receInfModel, ReceErrCdConst.ExpiredStartDateOdrErrCd, ReceErrCdConst.ExpiredStartDateOdrErrMsg, msg2, itemCd, sinDate: sinKouiCount.SinDate);
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
            foreach (var sinKouiCount in sinKouiCountList)
            {
                int sinDate = sinKouiCount.SinDate;
                long raiinNo = sinKouiCount.RaiinNo;
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


                            if (SystemConfig.Instance.HokensyuHandling == 0)
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
                            else if (SystemConfig.Instance.HokensyuHandling == 1)
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

                            if (SystemConfig.Instance.HokensyuHandling == 0)
                            {
                                // 同一に考える
                                if (hokenKbn == 4)
                                {
                                    //results.Add(2);
                                }
                            }
                            else if (SystemConfig.Instance.HokensyuHandling == 1)
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
                                _commandHandler.InsertReceCmtErr(receInfModel, ReceErrCdConst.SanteiCountCheckErrCd, ReceErrCdConst.SanteiCountCheckErrMsg, msg2, itemCd);
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
            foreach (var sinKouiCount in sinKouiCountList)
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
                _commandHandler.InsertReceCmtErr(receInfModel, ReceErrCdConst.FirstExamFeeCheckErrCd, ReceErrCdConst.FirstExamFeeCheckErrMsg, msg2);
            }
        }

        //E3005 check tokuzai item
        if (isCheckTokuzaiItem)
        {
            foreach (var sinKouiCount in sinKouiCountList)
            {
                if (sinKouiCount.SinKouiDetailModels.Any(p => p.ItemCd == ReceErrCdConst.TokuzaiItemCd))
                {
                    _commandHandler.InsertReceCmtErr(receInfModel, ReceErrCdConst.TokuzaiItemCheckErrCd, ReceErrCdConst.TokuzaiItemCheckErrMsg,
                                                    "（2017(H29)/04/01～使用不可）", ReceErrCdConst.TokuzaiItemCd, sinDate: sinKouiCount.SinDate);
                    continue;
                }
            }
        }

        //E3007 check patient age to use order
        if (isCheckItemAge)
        {
            List<string> checkedItemCds = new List<string>();
            int iBirthDay = receInfModel.Birthday;
            foreach (var sinKouiCount in sinKouiCountList)
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
                            _commandHandler.InsertReceCmtErr(receInfModel, ReceErrCdConst.ItemAgeCheckErrCd, ReceErrCdConst.ItemAgeCheckErrMsg, msg2, itemCd, sinDate: sinKouiCount.SinDate);
                        }

                        checkedItemCds.Add(itemCd);
                    }
                }
            }
        }

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
                                        _commandHandler.InsertReceCmtErr(receInfModel, ReceErrCdConst.CommentCheckErrCd, ReceErrCdConst.CommentCheckErrMsg, message, itemCd, cmtCd, 0);
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
            var addtionItems = _recalculationFinder.GetAddtionItems(receInfModel.PtId, receInfModel.SinYm, receInfModel.HokenId);
            if (addtionItems.Count > 0)
            {
                foreach (var item in addtionItems)
                {
                    _commandHandler.InsertReceCmtErr(receInfModel, ReceErrCdConst.AdditionItemErrCd, CIUtil.Copy(item.Text, 1, 100), "（" + CIUtil.SDateToShowSWDate(item.SinDate) + "）",
                        item.ItemCd + "," + item.DelItemCd, item.TermCnt + "," + item.TermSbt + "," + item.IsWarning, item.SinDate);
                }
            }
        }

        return newReceCheckErrList;
    }
    #endregion
}
