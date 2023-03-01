using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Constant;
using Domain.Models.SwapHoken;
using Helper.Common;
using UseCase.SwapHoken.Save;

namespace Interactor.SwapHoken
{
    public class SaveSwapHokenInteractor : ISaveSwapHokenInputPort
    {
        private readonly ISwapHokenRepository _swapHokenRepository;

        public SaveSwapHokenInteractor(ISwapHokenRepository repository)
        {
            _swapHokenRepository = repository;
        }

        public SaveSwapHokenOutputData Handle(SaveSwapHokenInputData inputData)
        {
            try
            {
                string message = string.Empty;
                if (inputData.HokenIdBefore <= 0)
                {
                    message = string.Format(ErrorMessage.MessageType_mSel01010, new string[] { "変換元保険" });
                    return new SaveSwapHokenOutputData(SaveSwapHokenStatus.SourceInsuranceHasNotSelected, message, TypeMessage.TypeMessageError);
                }
                    
                if (inputData.HokenIdAfter <= 0)
                {
                    message = string.Format(ErrorMessage.MessageType_mSel01010, new string[] { "変換先保険" });
                    return new SaveSwapHokenOutputData(SaveSwapHokenStatus.DesInsuranceHasNotSelected, message, TypeMessage.TypeMessageError);
                }

                if (inputData.StartDate > inputData.EndDate && inputData.StartDate > 0 && inputData.EndDate > 0)
                {
                    message = string.Format(ErrorMessage.MessageType_mInp00110, new string[] { "終了日", "開始日" });
                    return new SaveSwapHokenOutputData(SaveSwapHokenStatus.StartDateGreaterThanEndDate, message, TypeMessage.TypeMessageError);
                }

                string sBuff = string.Empty;
                if (inputData.StartDate > 0 && inputData.EndDate > 0)
                {
                    sBuff = CIUtil.SDateToShowSDate(inputData.StartDate) + " ～ " + CIUtil.SDateToShowSDate(inputData.EndDate);
                }
                else if (inputData.StartDate > 0 && inputData.EndDate == 0)
                {
                    sBuff = CIUtil.SDateToShowSDate(inputData.StartDate) + " 以降 ";
                }
                else if (inputData.StartDate == 0 && inputData.EndDate > 0)
                {
                    sBuff = CIUtil.SDateToShowSDate(inputData.EndDate) + " まで ";
                }

                if (string.IsNullOrEmpty(sBuff))
                {
                    if(!inputData.ConfirmInvalidIsShowConversionCondition)
                    {
                        if (!inputData.IsHokenPatternUsed) //IsShowConversionCondition
                        {
                            message = string.Format(ErrorMessage.MessageType_mDo00012, new string[] { "すべての期間が対象になります。", "保険変換" });
                            return new SaveSwapHokenOutputData(SaveSwapHokenStatus.InvalidIsShowConversionCondition, message, TypeMessage.TypeMessageWarning);
                        }
                        else
                        {
                            message = string.Format(ErrorMessage.MessageType_mDo00012, new string[] { "対象期間が指定されていないため、すべての期間が対象になります。", "保険変換" });
                            return new SaveSwapHokenOutputData(SaveSwapHokenStatus.InvalidIsShowConversionCondition, message, TypeMessage.TypeMessageWarning);
                        }
                    }
                    inputData.SetEndDate(99999999);
                    sBuff = "すべて";
                }
                else
                {
                    long count = _swapHokenRepository.CountOdrInf(inputData.HpId, inputData.PtId, inputData.HokenPidBefore, inputData.StartDate, inputData.EndDate);
                    if (count == 0)
                    {
                        message = string.Format(ErrorMessage.MessageType_mFree00030, new string[] { string.Format("変換元の保険は{0}に一度も使用されていないため、" + Environment.NewLine + "実行できません。", CIUtil.SDateToShowSDate(inputData.StartDate) + " ～ " + CIUtil.SDateToShowSDate(inputData.EndDate)) });
                        return new SaveSwapHokenOutputData(SaveSwapHokenStatus.CantExecCauseNotValidDate, message , TypeMessage.TypeMessageError );
                    }
                }

                if(!inputData.ConfirmSwapHoken)
                {
                    // 確認ﾒｯｾｰｼﾞ
                    message = string.Format(ErrorMessage.MessageType_mDo00010, new string[] { "次の条件で保険変換 " + "期間：       " + sBuff + Environment.NewLine
                    + "保険：       " + inputData.HokenBeforeName + "  → " + inputData.HokenAfterName + Environment.NewLine });
                    return new SaveSwapHokenOutputData(SaveSwapHokenStatus.ConfirmSwapHoken, message, TypeMessage.TypeMessageConfirmation);
                }

                var seikyuYms = _swapHokenRepository.GetListSeikyuYms(inputData.HpId, inputData.PtId, inputData.HokenPidBefore, inputData.StartDate, inputData.EndDate);
                var seiKyuPendingYms = _swapHokenRepository.GetSeikyuYmsInPendingSeikyu(inputData.HpId, inputData.PtId, seikyuYms, inputData.HokenIdBefore);

                bool swapHokenResult = _swapHokenRepository.SwapHokenParttern(inputData.HpId, inputData.PtId, inputData.HokenPidBefore, inputData.HokenPidAfter, inputData.StartDate, inputData.EndDate, inputData.UserId);

                if (!swapHokenResult)
                    return new SaveSwapHokenOutputData(SaveSwapHokenStatus.Failed, string.Empty, TypeMessage.TypeMessageError);

                if (seiKyuPendingYms.Count > 0)
                {
                    if (!_swapHokenRepository.ExistRaiinInfUsedOldHokenId(inputData.HpId, inputData.PtId, seikyuYms, inputData.HokenPidBefore))
                        _swapHokenRepository.UpdateReceSeikyu(inputData.HpId, inputData.PtId, seiKyuPendingYms, inputData.HokenIdBefore, inputData.HokenIdAfter, inputData.UserId);
                }

                return new SaveSwapHokenOutputData(SaveSwapHokenStatus.Successful, string.Empty, TypeMessage.TypeMessageSuccess);
            }
            finally
            {
                _swapHokenRepository.ReleaseResource();
            }
        }
    }
}
