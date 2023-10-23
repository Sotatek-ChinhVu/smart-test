using Domain.Models.Receipt;
using Helper.Constants;
using Helper.Messaging;
using Helper.Messaging.Data;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using Interactor.CalculateService;
using Interactor.Receipt;
using UseCase.MedicalExamination.Calculate;
using UseCase.Receipt.Recalculation;
using UseCase.ReceiptCheck.Recalculation;

namespace Interactor.ReceiptCheck
{
    public class ReceCheckRecalculationInteractor : IReceiptCheckRecalculationInputPort
    {
        private readonly ICalculateService _calculateService;
        private readonly IReceiptRepository _receiptRepository;
        private readonly ICommonReceRecalculation _commonReceRecalculation;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ITenantProvider _tenantProvider;
        private IMessenger? _messenger;

        public ReceCheckRecalculationInteractor(ITenantProvider tenantProvider, ICalculateService calculateService, IReceiptRepository receiptRepository, ICommonReceRecalculation commonReceRecalculation)
        {
            _calculateService = calculateService;
            _receiptRepository = receiptRepository;
            _commonReceRecalculation = commonReceRecalculation;
            _tenantProvider = tenantProvider;
            _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
        }

        public ReceiptCheckRecalculationOutputData Handle(ReceiptCheckRecalculationInputData inputData)
        {
            _messenger = inputData.Messenger;
            try
            {
                SendMessenger(new RecalculationStatus(false, CalculateStatusConstant.RecalculationCheckBox, 0, 0, "再計算中・・・", "NotConnectSocket"));
                _calculateService.RunCalculateMonth(
                    new CalculateMonthRequest()
                    {
                        HpId = inputData.HpId,
                        SeikyuYm = inputData.SeikyuYm,
                        PtIds = inputData.PtIds,
                        PreFix = inputData.UserId.ToString(),
                    }, CancellationToken.None);

                SendMessenger(new RecalculationStatus(false, CalculateStatusConstant.ReceiptAggregationCheckBox, 0, 0, "レセ集計中・・・", "NotConnectSocket"));
                _calculateService.ReceFutanCalculateMain(new ReceCalculateRequest(inputData.PtIds, inputData.SeikyuYm, string.Empty), CancellationToken.None);

                SendMessenger(new RecalculationStatus(false, CalculateStatusConstant.CheckErrorCheckBox, 0, 0, "レセチェック中・・・", "NotConnectSocket"));

                var receRecalculationList = _receiptRepository.GetReceRecalculationList(inputData.HpId, inputData.SeikyuYm, inputData.PtIds);
                int allCheckCount = receRecalculationList.Count;

                var success = _commonReceRecalculation.CheckErrorInMonth(inputData.HpId, inputData.PtIds, inputData.SeikyuYm, inputData.UserId, receRecalculationList, allCheckCount, _messenger, true);

                _receiptRepository.UpdateReceStatus(inputData.ReceStatus, inputData.HpId, inputData.UserId);

                return new ReceiptCheckRecalculationOutputData(success);
            }
            catch (Exception ex)
            {
                _loggingHandler.WriteLogExceptionAsync(ex);
                throw;
            }
            finally
            {
                SendMessenger(new RecalculationStatus(true, CalculateStatusConstant.ReceCheckMessage, 0, 0, string.Empty, "NotConnectSocket"));
                _commonReceRecalculation.ReleaseResource();
                _receiptRepository.ReleaseResource();
                _tenantProvider.DisposeDataContext();
                _loggingHandler.Dispose();
                _calculateService.ReleaseSource();
            }

        }

        private void SendMessenger(RecalculationStatus status)
        {
            _messenger!.Send(status);
        }
    }
}