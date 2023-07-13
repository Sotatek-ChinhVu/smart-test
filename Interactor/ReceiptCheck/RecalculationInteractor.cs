using Interactor.CalculateService;
using Interactor.Recalculation;
using UseCase.MedicalExamination.Calculate;
using UseCase.ReceiptCheck;
using Request = UseCase.Receipt.Recalculation;

namespace Interactor.ReceiptCheck
{
    public class RecalculationInteractor : IRecalculationInputPort
    {
        private readonly ICalculateService _calculateService;
        private readonly IRecalculation _recalculation;

        public RecalculationInteractor(ICalculateService calculateService, IRecalculation recalculation)
        {
            _calculateService = calculateService;
            _recalculation = recalculation;
        }

        public RecalculationOutputData Handle(RecalculationInputData inputData)
        {
            _calculateService.RunCalculateMonth(
                new Request.CalculateMonthRequest()
                {
                    HpId = inputData.HpId,
                    SeikyuYm = inputData.SeikyuYm,
                    PtIds = inputData.PtIds,
                    PreFix = ""
                });

            _calculateService.ReceFutanCalculateMain(new ReceCalculateRequest(inputData.PtIds, inputData.SeikyuYm));

            _calculateService.CheckErrorInMonth()
        }

    }
}
