using Domain.Models.Receipt;
using Domain.Models.SwapHoken;
using Helper.Constants;
using Helper.Enum;
using Helper.Messaging;
using Helper.Messaging.Data;
using Interactor.CalculateService;
using Interactor.Receipt;
using System.Text;
using UseCase.SwapHoken.Calculation;

namespace Interactor.SwapHoken
{
    public class CalculationSwapHokenInteractor : ICalculationSwapHokenInputPort
    {
        private readonly ISwapHokenRepository _swapHokenRepository;
        private readonly ICalcultateCustomerService _calcultateCustomerService;
        private readonly IReceiptRepository _receiptRepository;
        private readonly ICommonReceRecalculation _commonReceRecalculation;
        private IMessenger? _messenger;


        public CalculationSwapHokenInteractor(
            ISwapHokenRepository swapHokenRepository,
            ICalcultateCustomerService calcultateCustomerService,
            IReceiptRepository receiptRepository,
            ICommonReceRecalculation commonReceRecalculation)
        {
            _swapHokenRepository = swapHokenRepository;
            _calcultateCustomerService = calcultateCustomerService;
            _receiptRepository = receiptRepository;
            _commonReceRecalculation = commonReceRecalculation;
        }

        public CalculationSwapHokenOutputData Handle(CalculationSwapHokenInputData inputData)
        {
            _messenger = inputData.Messenger;
            try
            {
                List<int> seikyuYms = inputData.SeikyuYms;
                StringBuilder errorText = new();
                bool isStopCalc = false;
                int percentComplete = 0;
                int percentCompleteCalculate = 0;
                for (int i = 0; i < seikyuYms.Count; i++)
                {
                    int onceStepPercent = 1;
                    if (inputData.IsReCalculation)
                    {
                        isStopCalc = IsStopCalculate();
                        if (isStopCalc)
                        {
                            break;
                        }

                        percentComplete += onceStepPercent;
                        percentCompleteCalculate = percentComplete * 100 / (seikyuYms.Count * 3);
                        _calcultateCustomerService.RunCaculationPostAsync(TypeCalculate.RunCalculateMonth, new
                        {
                            HpId = inputData.HpId,
                            SeikyuYm = seikyuYms[i],
                            PtIds = new List<long>() { inputData.PtId }
                        }).Wait();
                        _messenger!.Send(new CalculationSwapHokenMessageStatus($"計算処理中.. 残り[{(seikyuYms.Count - i)}件]です", percentCompleteCalculate, false, false));
                    }

                    if (inputData.IsReceCalculation)
                    {
                        isStopCalc = IsStopCalculate();
                        if (isStopCalc)
                        {
                            break;
                        }

                        percentComplete += onceStepPercent;
                        percentCompleteCalculate = percentComplete * 100 / (seikyuYms.Count * 3);
                        _calcultateCustomerService.RunCaculationPostAsync(TypeCalculate.ReceFutanCalculateMain, new
                        {
                            HpId = inputData.HpId,
                            PtIds = new List<long>() { inputData.PtId },
                            SeikyuYm = seikyuYms[i]
                        }).Wait();
                        _messenger!.Send(new CalculationSwapHokenMessageStatus($"レセ集計中.. 残り[{(seikyuYms.Count - i)}件]です", percentCompleteCalculate, false, false));
                    }

                    if (inputData.IsReceCheckError)
                    {
                        isStopCalc = IsStopCalculate();
                        if (isStopCalc)
                        {
                            break;
                        }

                        percentComplete += onceStepPercent;
                        percentCompleteCalculate = percentComplete * 100 / (seikyuYms.Count * 3);
                        var receRecalculationList = _receiptRepository.GetReceRecalculationList(inputData.HpId, seikyuYms[i], new List<long> { inputData.PtId });
                        int allCheckCount = _receiptRepository.GetCountReceInfs(inputData.HpId, new List<long> { inputData.PtId }, seikyuYms[i]);
                        var resultCalculateInMonth = _commonReceRecalculation.CheckErrorInMonth(inputData.HpId, new List<long> { inputData.PtId }, seikyuYms[i], inputData.UserId, receRecalculationList, allCheckCount, _messenger, RunRecalculationStatus.RunCalculationSwapHoken);
                        if (inputData.IsReceCalculation)
                        {
                            errorText.Append(resultCalculateInMonth.ErrorText);
                        }
                        _messenger!.Send(new CalculationSwapHokenMessageStatus($"レセチェック処理中..残り[{(seikyuYms.Count - i)}件]です", percentCompleteCalculate, false, false));
                    }
                }

                for (int i = 0; i < seikyuYms.Count; i++)
                {
                    isStopCalc = IsStopCalculate();
                    if (isStopCalc)
                    {
                        break;
                    }
                    if (inputData.IsReceCheckError)
                    {
                        ResetStatusAfterCheckErr(inputData.HpId, inputData.PtId, seikyuYms[i], inputData.UserId);
                    }
                    else if (inputData.IsReCalculation)
                    {
                        ResetStatusAfterReCalc(inputData.HpId, inputData.UserId, inputData.PtId, seikyuYms[i]);
                    }
                }

                var resultErrorText = errorText.ToString();
                if (string.IsNullOrEmpty(resultErrorText))
                {
                    return new CalculationSwapHokenOutputData(CalculationSwapHokenStatus.Successful, resultErrorText);
                }
                else
                {
                    return new CalculationSwapHokenOutputData(CalculationSwapHokenStatus.Failed, resultErrorText);
                }
            }
            finally
            {
                _swapHokenRepository.ReleaseResource();
                _receiptRepository.ReleaseResource();
                _commonReceRecalculation.ReleaseResource();
            }
        }

        private bool IsStopCalculate() => _messenger!.SendAsync(new StopCalcStatus()).Result.Result;

        public void ResetStatusAfterCheckErr(int hpId, long ptId, int seikyuYm, int userId)
        {
            List<ReceInfForCheckErrSwapHokenModel> receInfModels = _receiptRepository.GetReceInforCheckErrForCalculateSwapHoken(hpId, new List<long> { ptId }, seikyuYm);

            if (receInfModels == null || receInfModels.Count == 0) return;

            List<ReceStatusModel> newReceStatus = new List<ReceStatusModel>();

            List<ReceStatusModel> updateReceStatus = new List<ReceStatusModel>();
            foreach (var receInfModel in receInfModels)
            {
                var receStatus = _receiptRepository.GetReceStatus(hpId, ptId, seikyuYm, receInfModel.SinYm, receInfModel.HokenId);
                bool hasError = _receiptRepository.HasErrorCheck(hpId, receInfModel.SinYm, receInfModel.PtId, receInfModel.HokenId);
                if (receStatus == null || receStatus.PtId == 0)
                {
                    if (hasError)
                    {
                        newReceStatus.Add(new ReceStatusModel(receInfModel.PtId, seikyuYm, receInfModel.HokenId, receInfModel.SinYm, 0, false, false, (int)ReceCheckStatusEnum.SystemPending, false));
                    }
                    continue;
                }

                if (hasError)
                {
                    if (receStatus.StatusKbn == (int)ReceCheckStatusEnum.UnConfirmed || receStatus.StatusKbn == (int)ReceCheckStatusEnum.TempComfirmed)
                    {
                        receStatus.SetStatusKbn((int)ReceCheckStatusEnum.SystemPending);
                    }
                }
                else
                {
                    if (receStatus.StatusKbn == (int)ReceCheckStatusEnum.SystemPending)
                    {
                        receStatus.SetStatusKbn((int)ReceCheckStatusEnum.UnConfirmed);
                    }
                }
                updateReceStatus.Add(receStatus);
            }

            _receiptRepository.SaveReceStatusCalc(newReceStatus, updateReceStatus, userId, hpId);
        }

        public bool ResetStatusAfterReCalc(int hpId, int userId, long ptId, int seikyuYm)
        {
            List<ReceInfForCheckErrSwapHokenModel> receInfModels = _receiptRepository.GetReceInforCheckErrForCalculateSwapHoken(hpId, new List<long> { ptId }, seikyuYm);
            List<ReceStatusModel> updateReceStatus = new List<ReceStatusModel>();
            foreach (var receInfModel in receInfModels)
            {
                var receStatus = _receiptRepository.GetReceStatus(hpId, ptId, seikyuYm, receInfModel.SinYm, receInfModel.HokenId);
                if (receStatus == null || receStatus.PtId == 0) continue;

                if (receStatus.StatusKbn == (int)ReceCheckStatusEnum.TempComfirmed)
                {
                    receStatus.SetStatusKbn((int)ReceCheckStatusEnum.UnConfirmed);
                }
                updateReceStatus.Add(receStatus);
            }
            return _receiptRepository.SaveReceStatusCalc(new List<ReceStatusModel>(), updateReceStatus, userId, hpId);
        }
    }
}
