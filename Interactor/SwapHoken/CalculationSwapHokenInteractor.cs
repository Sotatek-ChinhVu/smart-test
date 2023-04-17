using Domain.Models.SwapHoken;
using EventProcessor.Interfaces;
using Helper.Messaging.Data;
using Helper.Messaging;
using Interactor.CalculateService;
using UseCase.SwapHoken.Calculation;
using Domain.Models.InsuranceMst;
using Domain.Models.Receipt.Recalculation;
using Domain.Models.Receipt;
using System.Text;
using UseCase.ReceSeikyu.Save;
using Domain.Models.SystemConf;
using Domain.Models.MstItem;

namespace Interactor.SwapHoken
{
    public class CalculationSwapHokenInteractor : ICalculationSwapHokenInputPort
    {
        private readonly ISwapHokenRepository _swapHokenRepository;
        private readonly IEventProcessorService _eventProcessorService;
        private readonly ICalcultateCustomerService _calcultateCustomerService;
        private readonly IReceiptRepository _receiptRepository;
        private readonly ISystemConfRepository _systemConfRepository;
        private readonly IInsuranceMstRepository _insuranceMstRepository;
        private readonly IMstItemRepository _mstItemRepository;

        bool isStopCalc = false;

        public CalculationSwapHokenInteractor(ISwapHokenRepository repository, IEventProcessorService eventProcessorService, ICalcultateCustomerService calcultateCustomerService)
        {
            _swapHokenRepository = repository;
            _eventProcessorService = eventProcessorService;
            _calcultateCustomerService = calcultateCustomerService;
        }

        public CalculationSwapHokenOutputData Handle(CalculationSwapHokenInputData inputData)
        {
            try
            {
                List<int> seikyuYms = inputData.SeikyuYms;
                string errorText = string.Empty;
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
                        Messenger.Instance.Send(new CalculationSwapHokenMessageStatus($"計算処理中.. 残り[{(seikyuYms.Count - i)}件]です", percentCompleteCalculate, false, false));
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
                            PtIds = new List<long>() { inputData.PtId },
                            SeikyuYm = seikyuYms[i]
                        }).Wait();
                        Messenger.Instance.Send(new CalculationSwapHokenMessageStatus($"レセ集計中.. 残り[{(seikyuYms.Count - i)}件]です", percentCompleteCalculate, false, false));
                    }

                    if (inputData.IsReceCheckError)
                    {
                        isStopCalc = IsStopCalculate();
                        if (isStopCalc)
                        {
                            break;
                        }

                    }
                }

                for (int i = 0; i < seikyuYms.Count; i++)
                {

                }

                return new CalculationSwapHokenOutputData(CalculationSwapHokenStatus.Failed, string.Empty);
            }
            finally
            {
                _swapHokenRepository.ReleaseResource();
            }
        }

        private bool IsStopCalculate() => Messenger.Instance.SendAsync(new CalculationSwapHokenMessageStop()).Result.Result;

        private bool CheckErrorInMonth(SaveReceSeiKyuInputData inputData, List<ReceRecalculationModel> receRecalculationList, int allCheckCount)
        {
            List<ReceCheckErrModel> newReceCheckErrList = new();
            StringBuilder errorText = new();
            StringBuilder errorTextSinKouiCount = new();
            var receCheckOptList = GetReceCheckOptModelList(inputData.HpId);
            var ptIdList = receRecalculationList.Select(item => item.PtId).Distinct().ToList();
            var sinYmList = receRecalculationList.Select(item => item.SinYm).Distinct().ToList();
            var hokenIdList = receRecalculationList.Select(item => item.HokenId).Distinct().ToList();
            var kantokuCdValidList = receRecalculationList.Select(item => new IsKantokuCdValidModel(item.PtId, item.HokenId)).ToList();

            var allReceCheckErrList = _receiptRepository.GetReceCheckErrList(inputData.HpId, sinYmList, ptIdList, hokenIdList);
            var systemConfigList = _systemConfRepository.GetAllSystemConfig(inputData.HpId);
            var allSyobyoKeikaList = _receiptRepository.GetSyobyoKeikaList(inputData.HpId, sinYmList, ptIdList, hokenIdList);
            var allIsKantokuCdValidList = _insuranceMstRepository.GetIsKantokuCdValidList(inputData.HpId, kantokuCdValidList);

            int successCount = 1;
            foreach (var recalculationItem in receRecalculationList)
            {
                var statusCallBack = Messenger.Instance.SendAsync(new RecalculateInSeikyuPendingStop());
                isStopCalc = statusCallBack.Result.Result;
                if (isStopCalc)
                {
                    break;
                }
                List<BuiErrorModel> errorOdrInfDetails = new();
                var oldReceCheckErrList = allReceCheckErrList.Where(item => item.SinYm == recalculationItem.SinYm && item.PtId == recalculationItem.PtId && item.HokenId == recalculationItem.HokenId).ToList();
                var sinKouiCountList = _receiptRepository.GetSinKouiCountList(inputData.HpId, recalculationItem.SinYm, recalculationItem.PtId, recalculationItem.HokenId);
                List<string> itemCdList = new();
                foreach (var sinKouiCount in sinKouiCountList)
                {
                    itemCdList.AddRange(sinKouiCount.SinKouiDetailModels.Select(item => item.ItemCd).Distinct().ToList());
                }
                var tenMstByItemCdList = _mstItemRepository.GetTenMstList(inputData.HpId, itemCdList);
                newReceCheckErrList = CheckHokenError(recalculationItem, oldReceCheckErrList, newReceCheckErrList, receCheckOptList, sinKouiCountList);
                newReceCheckErrList = CheckByomeiError(inputData.HpId, recalculationItem, oldReceCheckErrList, newReceCheckErrList, receCheckOptList, sinKouiCountList, ref errorOdrInfDetails, systemConfigList);
                newReceCheckErrList = CheckOrderError(inputData.HpId, recalculationItem, oldReceCheckErrList, newReceCheckErrList, receCheckOptList, sinKouiCountList, tenMstByItemCdList, systemConfigList, itemCdList);
                newReceCheckErrList = CheckRosaiError(inputData.SinYm, ref errorText, recalculationItem, oldReceCheckErrList, newReceCheckErrList, sinKouiCountList, systemConfigList, allIsKantokuCdValidList, allSyobyoKeikaList);
                newReceCheckErrList = CheckAftercare(inputData.SinYm, recalculationItem, oldReceCheckErrList, newReceCheckErrList, systemConfigList, allSyobyoKeikaList);
                errorTextSinKouiCount = GetErrorTextSinKouiCount(inputData.SinYm, errorTextSinKouiCount, recalculationItem, sinKouiCountList);

                if (allCheckCount == successCount)
                {
                    break;
                }
                successCount++;
            }
            errorText.Append(errorTextSinKouiCount);
            errorText = GetErrorTextAfterCheck(inputData.HpId, inputData.SinYm, errorText, ptIdList, systemConfigList, receRecalculationList);

            return _receiptRepository.SaveNewReceCheckErrList(inputData.HpId, inputData.UserAct, newReceCheckErrList);
        }
    }
}
