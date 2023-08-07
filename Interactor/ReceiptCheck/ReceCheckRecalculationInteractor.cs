using Domain.CalculationInf;
using Domain.Models.Receipt;
using Domain.Models.Receipt.Recalculation;
using Domain.Models.SystemConf;
using Helper.Messaging;
using Helper.Messaging.Data;
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
        private readonly ICalculationInfRepository _calculationInfRepository;
        private readonly ISystemConfRepository _systemConfRepository;
        private readonly IReceiptRepository _receiptRepository;
        private readonly ICommonReceRecalculation _commonReceRecalculation;

        public ReceCheckRecalculationInteractor(ICalculateService calculateService, ICalculationInfRepository calculationInfRepository, ISystemConfRepository systemConfRepository, IReceiptRepository receiptRepository, ICommonReceRecalculation commonReceRecalculation)
        {
            _calculateService = calculateService;
            _calculationInfRepository = calculationInfRepository;
            _systemConfRepository = systemConfRepository;
            _receiptRepository = receiptRepository;
            _commonReceRecalculation = commonReceRecalculation;
        }

        public ReceiptCheckRecalculationOutputData Handle(ReceiptCheckRecalculationInputData inputData)
        {
            string errorText = string.Empty;
            try
            {
                SendMessenger(new RecalculationStatus(false, 1, 0, 0, "再計算中・・・", "NotConnectSocket"));
                _calculateService.RunCalculateMonth(
                    new CalculateMonthRequest()
                    {
                        HpId = inputData.HpId,
                        SeikyuYm = inputData.SeikyuYm,
                        PtIds = inputData.PtIds,
                        PreFix = ""
                    }, CancellationToken.None);

                SendMessenger(new RecalculationStatus(false, 2, 0, 0, "レセ集計中・・・", "NotConnectSocket"));
                _calculateService.ReceFutanCalculateMain(new ReceCalculateRequest(inputData.PtIds, inputData.SeikyuYm, string.Empty), CancellationToken.None);

                SendMessenger(new RecalculationStatus(false, 3, 0, 0, "レセチェック中・・・", "NotConnectSocket"));

                var receRecalculationList = _receiptRepository.GetReceRecalculationList(inputData.HpId, inputData.SeikyuYm, inputData.PtIds);
                int allCheckCount = receRecalculationList.Count;

                var success = _commonReceRecalculation.CheckErrorInMonth(inputData.HpId, inputData.PtIds, inputData.SeikyuYm, inputData.UserId, receRecalculationList, allCheckCount, true);

                _receiptRepository.UpdateReceStatus(inputData.ReceStatus, inputData.HpId, inputData.UserId);

                return new ReceiptCheckRecalculationOutputData(success);
            }
            finally
            {
                SendMessenger(new RecalculationStatus(true, 5, 0, 0, string.Empty, "NotConnectSocket"));
                _calculationInfRepository.ReleaseResource();
                _systemConfRepository.ReleaseResource();
                _receiptRepository.ReleaseResource();
            }

        }

        private void SendMessenger(RecalculationStatus status)
        {
            Messenger.Instance.Send(status);
        }
    }
}