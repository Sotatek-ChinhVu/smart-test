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

                Task.Run(() => { _calculateService.ReceFutanCalculateMain(new ReceCalculateRequest(ptIds, inputData.SeikyuYm)); });

                return new DeleteReceiptInfEditOutputData(DeleteReceiptInfStatus.Success);

            }
            finally
            {
                _calculationInfRepository.ReleaseResource();
            }
        }
    }
}
