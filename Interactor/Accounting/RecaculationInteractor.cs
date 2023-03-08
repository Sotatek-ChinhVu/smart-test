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
                var syunoStatus = _accountingRepository.CheckSyunoStatus(inputData.HpId, inputData.RaiinNo, inputData.PtId);

                if (!syunoStatus)
                    return new RecaculationOutputData(string.Empty, RecaculationStatus.Failed);

                var callCalculateInputData = new RecaculationInputDto(inputData.HpId, inputData.PtId, inputData.SinDate, 0, "SAI_");

                var result = _calculateService.RunCalculate(callCalculateInputData);
                if (string.IsNullOrEmpty(result))
                    return new RecaculationOutputData(result, RecaculationStatus.Successed);

                return new RecaculationOutputData(result, RecaculationStatus.Failed);
            }
            finally
            {
                _accountingRepository.ReleaseResource();
            }
        }

    }
}
