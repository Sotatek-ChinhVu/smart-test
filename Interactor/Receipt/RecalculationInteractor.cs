using Domain.Models.AuditLog;
using Domain.Models.Receipt;
using Domain.Models.Receipt.Recalculation;
using Helper.Constants;
using Helper.Messaging;
using Helper.Messaging.Data;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using Interactor.CalculateService;
using UseCase.MedicalExamination.Calculate;
using UseCase.Receipt.Recalculation;

namespace Interactor.Receipt;

public class RecalculationInteractor : IRecalculationInputPort
{
    private readonly IReceiptRepository _receiptRepository;
    private readonly ICalculateService _calculateService;
    private readonly ICommonReceRecalculation _commonReceRecalculation;
    private readonly IAuditLogRepository _auditLogRepository;
    private readonly ILoggingHandler _loggingHandler;
    private readonly ITenantProvider _tenantProvider;
    private IMessenger? _messenger;

    bool isStopCalc = false;

    public RecalculationInteractor(ITenantProvider tenantProvider, IReceiptRepository receiptRepository, ICommonReceRecalculation commonReceRecalculation, ICalculateService calculateRepository, IAuditLogRepository auditLogRepository)
    {
        _receiptRepository = receiptRepository;
        _commonReceRecalculation = commonReceRecalculation;
        _calculateService = calculateRepository;
        _auditLogRepository = auditLogRepository;
        _tenantProvider = tenantProvider;
        _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
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
                success = RunCalculateMonth(inputData.HpId, inputData.UserId, inputData.SinYm, inputData.PtIdList, inputData.UniqueKey, inputData.CancellationToken);

                // Check next step
                while (true)
                {
                    if (CheckAllowNextStep())
                    {
                        break;
                    }
                }
            }

            // Check is stop progerss
            var statusCallBack = _messenger!.SendAsync(new StopCalcStatus());
            isStopCalc = statusCallBack.Result.Result;
            if (isStopCalc)
            {
                success = false;
            }

            // run Receipt Aggregation
            if (success && !isStopCalc && inputData.IsReceiptAggregationCheckBox)
            {
                success = ReceFutanCalculateMain(inputData.SinYm, inputData.PtIdList, inputData.UniqueKey, inputData.CancellationToken);

                // Check next step
                while (true)
                {
                    if (CheckAllowNextStep())
                    {
                        break;
                    }
                }
            }

            // Check is stop progerss
            statusCallBack = _messenger!.SendAsync(new StopCalcStatus());
            isStopCalc = statusCallBack.Result.Result;
            if (isStopCalc)
            {
                success = false;
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
                SendMessager(new RecalculationStatus(true, CalculateStatusConstant.None, 0, 0, string.Empty, string.Empty));
            }
            AddAuditLog(inputData.HpId, inputData.UserId, inputData.SinYm, inputData.IsRecalculationCheckBox, inputData.IsReceiptAggregationCheckBox, inputData.IsCheckErrorCheckBox, inputData.PtIdList.Any());
            return new RecalculationOutputData(success);
        }
        catch (Exception ex)
        {
            _loggingHandler.WriteLogExceptionAsync(ex);
            throw;
        }
        finally
        {
            _commonReceRecalculation.ReleaseResource();
            _receiptRepository.ReleaseResource();
            _auditLogRepository.ReleaseResource();
            _calculateService.ReleaseSource();
            _tenantProvider.DisposeDataContext();
            _loggingHandler.Dispose();
        }
    }

    private bool RunCalculateMonth(int hpId, int userId, int seikyuYm, List<long> ptInfList, string uniqueKey, CancellationToken cancellationToken)
    {
        SendMessager(new RecalculationStatus(false, CalculateStatusConstant.RecalculationCheckBox, 0, 0, "StartCalculateMonth", string.Empty));
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
            PreFix = userId.ToString(),
            UniqueKey = uniqueKey
        }, cancellationToken);
        return true;
    }

    private bool ReceFutanCalculateMain(int seikyuYm, List<long> ptInfList, string uniqueKey, CancellationToken cancellationToken)
    {
        SendMessager(new RecalculationStatus(false, CalculateStatusConstant.ReceiptAggregationCheckBox, 0, 0, "StartFutanCalculateMain", string.Empty));
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

    private bool CheckAllowNextStep()
    {
        var allowNextStep = _messenger!.SendAsync(new AllowNextStepStatus());
        return allowNextStep.Result.Result;
    }

    private void AddAuditLog(int hpId, int userId, int sinDate, bool recalculation, bool receiptAggregation, bool isCheckError, bool isSpecifiedPt)
    {
        var hosoku = string.Format("CALC:{0},SUMRECE:{1},CHECK:{2},PT:{3}",
                                                   recalculation ? 1 : 0,
                                                   receiptAggregation ? 1 : 0,
                                                   isCheckError ? 1 : 0,
                                                   isSpecifiedPt ? 1 : 0);
        var arg = new ArgumentModel(
                        EventCode.Recalculation,
                        0,
                        sinDate,
                        0,
                        0,
                        0,
                        0,
                        0,
        hosoku
        );

        _auditLogRepository.AddAuditTrailLog(hpId, userId, arg);
    }
}