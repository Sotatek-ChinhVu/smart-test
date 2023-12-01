using Domain.CalculationInf;
using Interactor.CalculateService;
using UseCase.MedicalExamination.Calculate;
using UseCase.ReceiptCheck.ReceiptInfEdit;

namespace Interactor.ReceiptCheck
{
    public class DeleteReceiptInfEditInteractor : IDeleteReceiptInfEditInputPort
    {
        private readonly ICalculateService _calculateService;
        private readonly ICalculationInfRepository _calculationInfRepository;

        public DeleteReceiptInfEditInteractor(ICalculateService calculateService, ICalculationInfRepository calculationInfRepository)
        {
            _calculateService = calculateService;
            _calculationInfRepository = calculationInfRepository;
        }

        public DeleteReceiptInfEditOutputData Handle(DeleteReceiptInfEditInputData inputData)
        {
            try
            {
                _calculationInfRepository.DeleteReceiptInfEdit(inputData.HpId, inputData.UserId, inputData.SeikyuYm, inputData.PtId, inputData.SinYm, inputData.HokenId);

                var ptIds = new List<long>() { inputData.PtId };

                _calculateService.ReceFutanCalculateMain(new ReceCalculateRequest(ptIds, inputData.SeikyuYm, string.Empty));

                return new DeleteReceiptInfEditOutputData(DeleteReceiptInfStatus.Successed);

            }
            finally
            {
                _calculationInfRepository.ReleaseResource();
                _calculateService.ReleaseSource();
            }
        }
    }
}
