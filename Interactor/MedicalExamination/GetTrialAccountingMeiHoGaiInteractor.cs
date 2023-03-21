using Domain.Models.MedicalExamination;
using Domain.Models.Reception;
using Interactor.CalculateService;
using UseCase.MedicalExamination.GetCheckedOrder;
using UseCase.MedicalExamination.TrailAccounting;

namespace Interactor.MedicalExamination
{
    public class GetTrialAccountingMeiHoGaiInteractor : IGetTrialAccountingMeiHoGaiInputPort
    {
        private readonly ICalculateService _calculateRepository;
        private readonly IReceptionRepository _receptionRepository;
        private readonly IMedicalExaminationRepository _medicalExaminationRepository;

        public GetTrialAccountingMeiHoGaiInteractor(ICalculateService calculateRepository, IReceptionRepository receptionRepository)
        {
            _calculateRepository = calculateRepository;
            _receptionRepository = receptionRepository;
        }

        public GetTrialAccountingMeiHoGaiOutputData Handle(GetTrialAccountingMeiHoGaiInputData inputData)
        {
            try
            {

                var raiinInf = _receptionRepository.Get(inputData.RaiinNo);
                var requestRaiinInf = new ReceptionItem(raiinInf);
                var runTraialCalculateRequest = new RunTraialCalculateRequest(
                                inputData.HpId,
                                inputData.PtId,
                                inputData.SinDate,
                                inputData.RaiinNo,
                                inputData.OdrInfItems,
                                requestRaiinInf,
                                true
                            );

                var runTrialCalculate = _calculateRepository.RunTrialCalculate(runTraialCalculateRequest);
                return new GetTrialAccountingMeiHoGaiOutputData(GetTrialAccountingMeiHoGaiStatus.Successed);
            }
            finally
            {
                _receptionRepository.ReleaseResource();
            }

        }
    }
}
