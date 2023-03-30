using Domain.Models.Accounting;
using Interactor.CalculateService;
using UseCase.Accounting.Recaculate;

namespace Interactor.Accounting
{
    public class RecaculationInteractor : IRecaculationInputPort
    {
        private readonly ICalculateService _calculateService;
        private readonly IAccountingRepository _accountingRepository;
        public RecaculationInteractor(ICalculateService calculateService, IAccountingRepository accountingRepository)
        {
            _calculateService = calculateService;
            _accountingRepository = accountingRepository;
        }

        public RecaculationOutputData Handle(RecaculationInputData inputData)
        {
            try
            {
                var callCalculateInputData = new RecaculationInputDto(inputData.HpId, inputData.PtId, inputData.SinDate, 0, "SAI_");

                var responseStatus = _calculateService.RunCalculate(callCalculateInputData);
                if (responseStatus)
                    return new RecaculationOutputData(RecaculationStatus.Successed);

                return new RecaculationOutputData(RecaculationStatus.Failed);
            }
            finally
            {
                _accountingRepository.ReleaseResource();
            }
        }

    }
}
