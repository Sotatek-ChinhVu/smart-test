using Domain.Constant;
using Domain.Models.SwapHoken;
using EventProcessor.Interfaces;
using EventProcessor.Model;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using UseCase.SwapHoken.Save;

namespace Interactor.SwapHoken
{
    public class SaveSwapHokenInteractor : ISaveSwapHokenInputPort
    {
        private readonly ISwapHokenRepository _swapHokenRepository;
        private readonly IEventProcessorService _eventProcessorService;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ITenantProvider _tenantProvider;

        public SaveSwapHokenInteractor(ITenantProvider tenantProvider, ISwapHokenRepository repository, IEventProcessorService eventProcessorService)
        {
            _swapHokenRepository = repository;
            _eventProcessorService = eventProcessorService;
            _tenantProvider = tenantProvider;
            _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
        }

        public SaveSwapHokenOutputData Handle(SaveSwapHokenInputData inputData)
        {
            try
            {
                string message = string.Empty;
                if (inputData.HokenIdBefore <= 0)
                {
                    message = string.Format(ErrorMessage.MessageType_mSel01010, new string[] { "変換元保険" });
                    return new SaveSwapHokenOutputData(SaveSwapHokenStatus.SourceInsuranceHasNotSelected, message, TypeMessage.TypeMessageError, new List<int>());
                }

                if (inputData.HokenIdAfter <= 0)
                {
                    message = string.Format(ErrorMessage.MessageType_mSel01010, new string[] { "変換先保険" });
                    return new SaveSwapHokenOutputData(SaveSwapHokenStatus.DesInsuranceHasNotSelected, message, TypeMessage.TypeMessageError, new List<int>());
                }

                if (inputData.StartDate > inputData.EndDate && inputData.StartDate > 0 && inputData.EndDate > 0)
                {
                    message = string.Format(ErrorMessage.MessageType_mInp00110, "終了日", "開始日");
                    return new SaveSwapHokenOutputData(SaveSwapHokenStatus.StartDateGreaterThanEndDate, message, TypeMessage.TypeMessageError, new List<int>());
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
                    if (!inputData.ConfirmInvalidIsShowConversionCondition)
                    {
                        if (!inputData.IsHokenPatternUsed) //IsShowConversionCondition
                        {
                            message = "すべての期間が対象になります。" + Environment.NewLine + "保険変換を実行しますか？";

                            return new SaveSwapHokenOutputData(SaveSwapHokenStatus.InvalidIsShowConversionCondition, message, TypeMessage.TypeMessageWarning, new List<int>());
                        }
                        else
                        {
                            message = "対象期間が指定されていないため、すべての期間が対象になります。" + Environment.NewLine + "保険変換を実行しますか？";
                            return new SaveSwapHokenOutputData(SaveSwapHokenStatus.InvalidIsShowConversionCondition, message, TypeMessage.TypeMessageWarning, new List<int>());
                        }
                    }
                    sBuff = "すべて";
                }
                else
                {
                    long count = _swapHokenRepository.CountOdrInf(inputData.HpId, inputData.PtId, inputData.HokenPidBefore, inputData.StartDate, inputData.EndDate);
                    if (count == 0)
                    {
                        message = string.Format("変換元の保険は{0}に一度も使用されていないため、実行できません。", CIUtil.SDateToShowSDate(inputData.StartDate) + " ～ " + CIUtil.SDateToShowSDate(inputData.EndDate));
                        return new SaveSwapHokenOutputData(SaveSwapHokenStatus.CantExecCauseNotValidDate, message, TypeMessage.TypeMessageError, new List<int>());
                    }
                }

                if (inputData.EndDate == 0)
                {
                    inputData.SetEndDate(99999999);
                }

                if (!inputData.ConfirmSwapHoken)
                {
                    // 確認ﾒｯｾｰｼﾞ
                    message = "次の条件で保険変換を実行しますか？" + Environment.NewLine
                    + "期間：       " + sBuff + Environment.NewLine
                    + "保険：       " + inputData.HokenBeforeName + "  → " + inputData.HokenAfterName;
                    return new SaveSwapHokenOutputData(SaveSwapHokenStatus.ConfirmSwapHoken, message, TypeMessage.TypeMessageConfirmation, new List<int>());
                }

                _eventProcessorService.DoEvent(new List<EventProcessor.Model.ArgumentModel>()
                {
                    new ArgumentModel(inputData.HpId,
                                      inputData.UserId,
                                      EventCode.PatientInfSwitchHokenPattern,
                                      inputData.PtId,
                                      0,
                                      0,
                                      string.Format("pid:{0}→{1} term:{2}-{3} calc:{4} receCalc:{5} check:{6} printer:{7}",
                                                     inputData.HokenPidBefore,
                                                     inputData.HokenPidAfter,
                                                     inputData.StartDate,
                                                     inputData.EndDate,
                                                     inputData.IsReCalculation ? 1 : 0,
                                                     inputData.IsReceCalculation ? 1 : 0,
                                                     inputData.IsReceCheckError ? 1 : 0,
                                                     string.Empty))
                });

                var seikyuYms = _swapHokenRepository.GetListSeikyuYms(inputData.HpId, inputData.PtId, inputData.HokenPidBefore, inputData.StartDate, inputData.EndDate);
                var seiKyuPendingYms = _swapHokenRepository.GetSeikyuYmsInPendingSeikyu(inputData.HpId, inputData.PtId, seikyuYms, inputData.HokenIdBefore);

                bool swapHokenResult = _swapHokenRepository.SwapHokenParttern(inputData.HpId, inputData.PtId, inputData.HokenPidBefore, inputData.HokenPidAfter, inputData.StartDate, inputData.EndDate, inputData.UserId);

                if (!swapHokenResult)
                {
                    return new SaveSwapHokenOutputData(SaveSwapHokenStatus.Failed, string.Empty, TypeMessage.TypeMessageError, new List<int>());
                }

                if (seiKyuPendingYms.Count > 0)
                {
                    if (!_swapHokenRepository.ExistRaiinInfUsedOldHokenId(inputData.HpId, inputData.PtId, seikyuYms, inputData.HokenPidBefore))
                    {
                        _swapHokenRepository.UpdateReceSeikyu(inputData.HpId, inputData.PtId, seikyuYms, inputData.HokenIdBefore, inputData.HokenIdAfter, inputData.UserId);
                    }
                    seikyuYms.AddRange(seiKyuPendingYms);
                    seikyuYms = seikyuYms.Distinct().ToList();
                }
                return new SaveSwapHokenOutputData(SaveSwapHokenStatus.Successful, string.Empty, TypeMessage.TypeMessageSuccess, seikyuYms);
            }
            catch (Exception ex)
            {
                _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
            finally
            {
                _swapHokenRepository.ReleaseResource();
                _loggingHandler.Dispose();
            }
        }
    }
}
