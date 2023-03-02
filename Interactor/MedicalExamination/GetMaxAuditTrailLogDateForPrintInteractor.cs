using Domain.Models.MedicalExamination;
using UseCase.MedicalExamination.GetMaxAuditTrailLogDateForPrint;

namespace Interactor.MedicalExamination
{
    public class GetMaxAuditTrailLogDateForPrintInteractor : IGetMaxAuditTrailLogDateForPrintInputPort
    {
        private readonly IMedicalExaminationRepository _medicalRepository;

        public GetMaxAuditTrailLogDateForPrintInteractor(IMedicalExaminationRepository todayOdrRepository)
        {
            _medicalRepository = todayOdrRepository;
        }

        public GetMaxAuditTrailLogDateForPrintOutputData Handle(GetMaxAuditTrailLogDateForPrintInputData inputData)
        {
            try
            {
                if (inputData.PtId < 0)
                {
                    return new GetMaxAuditTrailLogDateForPrintOutputData(new(), GetMaxAuditTrailLogDateForPrintStatus.InvalidPtId);
                }
                if (inputData.RaiinNo < 0)
                {
                    return new GetMaxAuditTrailLogDateForPrintOutputData(new(), GetMaxAuditTrailLogDateForPrintStatus.InvalidRaiinNo);
                }
                if (inputData.SinDate < 0)
                {
                    return new GetMaxAuditTrailLogDateForPrintOutputData(new(), GetMaxAuditTrailLogDateForPrintStatus.InvalidSinDate);
                }
                var result = _medicalRepository.GetMaxAuditTrailLogDateForPrint(inputData.PtId, inputData.SinDate, inputData.RaiinNo);

                return new GetMaxAuditTrailLogDateForPrintOutputData(result, GetMaxAuditTrailLogDateForPrintStatus.Successed);
            }
            finally
            {
                _medicalRepository.ReleaseResource();
            }
        }
    }
}
