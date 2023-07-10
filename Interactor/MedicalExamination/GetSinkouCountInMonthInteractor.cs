using Domain.Models.MedicalExamination;
using UseCase.MedicalExamination.GetSinkouCountInMonth;

namespace Interactor.MedicalExamination
{
    public class GetSinkouCountInMonthInteractor : IGetSinkouCountInMonthInputPort
    {
        private readonly IMedicalExaminationRepository _medicalExaminationRepository;

        public GetSinkouCountInMonthInteractor(IMedicalExaminationRepository medicalExaminationRepository)
        {
            _medicalExaminationRepository = medicalExaminationRepository;
        }

        public GetSinkouCountInMonthOutputData Handle(GetSinkouCountInMonthInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new GetSinkouCountInMonthOutputData(GetSinkouCountInMonthStatus.InvalidHpId, new());
                }
                if (inputData.SinDate <= 0)
                {
                    return new GetSinkouCountInMonthOutputData(GetSinkouCountInMonthStatus.InvalidSinDate, new());
                }
                if (inputData.PtId <= 0)
                {
                    return new GetSinkouCountInMonthOutputData(GetSinkouCountInMonthStatus.InvalidPtId, new());
                }

                var result = _medicalExaminationRepository.GetSinkouCountInMonth(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.ItemCd);

                return new GetSinkouCountInMonthOutputData(GetSinkouCountInMonthStatus.Successed, result);
            }
            finally
            {
                _medicalExaminationRepository.ReleaseResource();
            }
        }
    }
}
