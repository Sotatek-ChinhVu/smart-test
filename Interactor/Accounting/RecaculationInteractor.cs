using Helper.Enum;
using Interactor.CalculateService;
using UseCase.Accounting.Recaculate;

namespace Interactor.Accounting
{
    public class RecaculationInteractor : IRecaculationInputPort
    {
        private readonly ICalculateService _calculateService;
        public RecaculationInteractor(ICalculateService calculateService)
        {
            _calculateService = calculateService;
        }

        public RecaculationOutputData Handle(RecaculationInputData inputData)
        {
            var callCalculateInputData = new RecaculationInputDto(inputData.HpId, inputData.PtId, inputData.SinDate, 0, "SAI_");
            var result = _calculateService.RunCalculate(CalculateApiPath.RunCalculate, callCalculateInputData);

            if (!result) return new RecaculationOutputData(RecaculationStatus.Failed);

            return new RecaculationOutputData(RecaculationStatus.Successed);
        }

    }
}
