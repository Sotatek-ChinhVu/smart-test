using Domain.Models.Diseases;
using Domain.Models.DrugDetail;
using Domain.Models.InsuranceMst;
using Domain.Models.MstItem;
using Domain.Models.OrdInfs;
using Domain.Models.Receipt;
using Domain.Models.ReceSeikyu;
using Domain.Models.SystemConf;
using Domain.Models.TodayOdr;
using Helper.Messaging;
using Helper.Messaging.Data;
using Infrastructure.Repositories;
using Interactor.CalculateService;
using Interactor.CommonChecker.CommonMedicalCheck;
using UseCase.MedicalExamination.Calculate;
using UseCase.Receipt.Recalculation;

namespace Interactor.Receipt;

public class RecalculationInteractor : IRecalculationInputPort
{
    private readonly IReceiptRepository _receiptRepository;
    private readonly ICalculateService _calculateRepository;
    private readonly ICommonReceRecalculation _commonReceRecalculation;

    bool isStopCalc = false;

    public RecalculationInteractor(IReceiptRepository receiptRepository, ICalculateService calculateService, ICommonReceRecalculation commonReceRecalculation)
    {
        _receiptRepository = receiptRepository;
        _calculateRepository = calculateService;
        _commonReceRecalculation = commonReceRecalculation;
    }

    public RecalculationOutputData Handle(RecalculationInputData inputData)
    {
        try
        {
            bool success = true;
            var ptInfCount = inputData.PtIdList.Count;
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
            if (success && !isStopCalc && inputData.IsCheckErrorCheckBox)
            {
                var receRecalculationList = _receiptRepository.GetReceRecalculationList(inputData.HpId, inputData.SinYm, inputData.PtIdList);
                int allCheckCount = receRecalculationList.Count;
                
                success = _commonReceRecalculation.CheckErrorInMonth(inputData.HpId, inputData.PtIdList, inputData.SinYm, inputData.UserId, receRecalculationList, allCheckCount);
            }

            if (!inputData.IsCheckErrorCheckBox && !inputData.IsReceiptAggregationCheckBox && !inputData.IsRecalculationCheckBox)
            {
                SendMessager(new RecalculationStatus(true, 0, 0, 0, string.Empty, string.Empty));
            }
            return new RecalculationOutputData(success);
        }
        finally
        {
            _receiptRepository.ReleaseResource();
        }
    }

    private bool RunCalculateMonth(int hpId, int seikyuYm, List<long> ptInfList, string uniqueKey, CancellationToken cancellationToken)
    {
        SendMessager(new RecalculationStatus(false, 1, 0, 0, "StartCalculateMonth", string.Empty));
        var statusCallBack = Messenger.Instance.SendAsync(new StopCalcStatus());
        isStopCalc = statusCallBack.Result.Result;
        if (isStopCalc)
        {
            return false;
        }
        _calculateRepository.RunCalculateMonth(new CalculateMonthRequest()
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
        var statusCallBack = Messenger.Instance.SendAsync(new StopCalcStatus());
        isStopCalc = statusCallBack.Result.Result;
        if (isStopCalc)
        {
            return false;
        }
        _calculateRepository.ReceFutanCalculateMain(new ReceCalculateRequest(ptInfList, seikyuYm, uniqueKey), cancellationToken);
        return true;
    }

    private void SendMessager(RecalculationStatus status)
    {
        Messenger.Instance.Send(status);
    }
}