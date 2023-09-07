using Domain.Models.Receipt;
using Domain.Models.Receipt.Recalculation;
using Helper.Messaging;
using Helper.Messaging.Data;
using Interactor.CalculateService;
using UseCase.MedicalExamination.Calculate;
using UseCase.Receipt.Recalculation;

namespace Interactor.Receipt;

public class RecalculationInteractor : IRecalculationInputPort
{
    private readonly IReceiptRepository _receiptRepository;
    private readonly ICalculateService _calculateService;
    private readonly ICommonReceRecalculation _commonReceRecalculation;
    private IMessenger? _messenger;

    bool isStopCalc = false;

    public RecalculationInteractor(IReceiptRepository receiptRepository, ICommonReceRecalculation commonReceRecalculation, ICalculateService calculateRepository)
    {
        _receiptRepository = receiptRepository;
        _commonReceRecalculation = commonReceRecalculation;
        _calculateService = calculateRepository;
    }

    public RecalculationOutputData Handle(RecalculationInputData inputData)
    {
        _messenger = inputData.Messenger;
        try
        {
            bool success = true;
            // run Recalculation
            if (!isStopCalc && inputData.IsRecalculationCheckBox)
            {
                success = RunCalculateMonth(inputData.HpId, inputData.SinYm, inputData.PtIdList, inputData.UniqueKey, inputData.CancellationToken);
            }

            // run Receipt Aggregation
            if (success && !isStopCalc && inputData.IsReceiptAggregationCheckBox)
            {
                success = ReceFutanCalculateMain(inputData.SinYm, inputData.PtIdList, inputData.UniqueKey, inputData.CancellationToken);
            }

            // check error in month
            List<ReceRecalculationModel> receRecalculationList = new();
            if (success && !isStopCalc && (inputData.IsCheckErrorCheckBox || inputData.IsReceiptAggregationCheckBox))
            {
                receRecalculationList = _receiptRepository.GetReceRecalculationList(inputData.HpId, inputData.SinYm, inputData.PtIdList);
                int allCheckCount = receRecalculationList.Count;

                success = _commonReceRecalculation.CheckErrorInMonth(inputData.HpId, inputData.PtIdList, inputData.SinYm, inputData.UserId, receRecalculationList, allCheckCount, _messenger, receCheckCalculate: false, isReceiptAggregationCheckBox: inputData.IsReceiptAggregationCheckBox, inputData.IsCheckErrorCheckBox);
            }

            // resetStatus
            if (success)
            {
                if (inputData.IsCheckErrorCheckBox)
                {
                    _receiptRepository.ResetStatusAfterCheckErr(inputData.HpId, inputData.UserId, inputData.SinYm, receRecalculationList);
                }
                else if (inputData.IsRecalculationCheckBox)
                {
                    _receiptRepository.ResetStatusAfterReCalc(inputData.HpId, inputData.PtIdList, inputData.SinYm);
                }
            }

            if (!inputData.IsCheckErrorCheckBox && !inputData.IsReceiptAggregationCheckBox && !inputData.IsRecalculationCheckBox)
            {
                SendMessager(new RecalculationStatus(true, 0, 0, 0, string.Empty, string.Empty));
            }
            return new RecalculationOutputData(success);
        }
        finally
        {
            _commonReceRecalculation.ReleaseResource();
            _receiptRepository.ReleaseResource();
        }
    }

    private bool RunCalculateMonth(int hpId, int seikyuYm, List<long> ptInfList, string uniqueKey, CancellationToken cancellationToken)
    {
        SendMessager(new RecalculationStatus(false, 1, 0, 0, "StartCalculateMonth", string.Empty));
        var statusCallBack = _messenger!.SendAsync(new StopCalcStatus());
        isStopCalc = statusCallBack.Result.Result;
        if (isStopCalc)
        {
            return false;
        }
        _calculateService.RunCalculateMonth(new CalculateMonthRequest()
        {
            HpId = hpId,
            PtIds = ptInfList,
            SeikyuYm = seikyuYm,
            UniqueKey = uniqueKey
        }, cancellationToken);
        return true;
    }

    private bool ReceFutanCalculateMain(int seikyuYm, List<long> ptInfList, string uniqueKey, CancellationToken cancellationToken)
    {
        SendMessager(new RecalculationStatus(false, 2, 0, 0, "StartFutanCalculateMain", string.Empty));
        var statusCallBack = _messenger!.SendAsync(new StopCalcStatus());
        isStopCalc = statusCallBack.Result.Result;
        if (isStopCalc)
        {
            return false;
        }
        _calculateService.ReceFutanCalculateMain(new ReceCalculateRequest(ptInfList, seikyuYm, uniqueKey), cancellationToken);
        return true;
    }

    private void SendMessager(RecalculationStatus status)
    {
        _messenger!.Send(status);
    }
}