using Domain.Models.Receipt;
using Domain.Models.Receipt.Recalculation;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using UseCase.Receipt.Recalculation;

namespace Interactor.Receipt;

public class RecalculationInteractor : IRecalculationInputPort
{
    private readonly IReceiptRepository _receiptRepository;
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
    public RecalculationInteractor(IReceiptRepository receiptRepository)
    {
        _receiptRepository = receiptRepository;
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
                var oldReceCheckErrList = _receiptRepository.GetReceCheckErrList(inputData.HpId, recalculationItem.SinYm, recalculationItem.PtId, recalculationItem.HokenId);
                var sinKouiCountList = _receiptRepository.GetSinKouiCountList(inputData.HpId, recalculationItem.SinYm, recalculationItem.PtId, recalculationItem.HokenId);
                newReceCheckErrList = CheckHokenError(recalculationItem, oldReceCheckErrList, newReceCheckErrList, receCheckOptList, sinKouiCountList);
            }
            var count = newReceCheckErrList.Count;
            return new RecalculationOutputData(RecalculationStatus.Successed, errorMessage + count.ToString());
        }
        finally
        {
            _receiptRepository.ReleaseResource();
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

    #endregion
}
