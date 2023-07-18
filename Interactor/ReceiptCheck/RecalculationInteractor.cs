using Domain.CalculationInf;
using Interactor.CalculateService;
using UseCase.MedicalExamination.Calculate;
using UseCase.ReceiptCheck;
using Request = UseCase.Receipt.Recalculation;

namespace Interactor.ReceiptCheck
{
    public class RecalculationInteractor : IRecalculationInputPort
    {
        private readonly ICalculateService _calculateService;
        private readonly ICalculationInfRepository _calculationInfRepository;

        public RecalculationInteractor(ICalculateService calculateService, ICalculationInfRepository calculationInfRepository)
        {
            _calculateService = calculateService;
            _calculationInfRepository = calculationInfRepository;
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

            _calculationInfRepository.CheckErrorInMonth(inputData.HpId, inputData.SeikyuYm, inputData.PtIds);
        }

    }
}
