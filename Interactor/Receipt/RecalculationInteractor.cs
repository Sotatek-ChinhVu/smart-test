using Domain.Models.Receipt;
using Domain.Models.Receipt.Recalculation;
using Helper.Constants;
using UseCase.Receipt.Recalculation;

namespace Interactor.Receipt;

public class RecalculationInteractor : IRecalculationInputPort
{
    private readonly IReceiptRepository _receiptRepository;

    public RecalculationInteractor(IReceiptRepository receiptRepository)
    {
        _receiptRepository = receiptRepository;
    }

    public RecalculationOutputData Handle(RecalculationInputData inputData)
    {
        try
        {
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
                //_receiptRepository.ClearReceCmtErr(inputData.HpId, recalculationItem.PtId, recalculationItem.HokenId, recalculationItem.SinYm);
                var sinKouiCountList = _receiptRepository.GetSinKouiCountList(inputData.HpId, recalculationItem.SinYm, recalculationItem.PtId, recalculationItem.HokenId);
            }
            return new RecalculationOutputData(RecalculationStatus.Successed, errorMessage);
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
    #endregion
}
