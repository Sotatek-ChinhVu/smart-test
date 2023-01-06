using Domain.Models.MedicalExamination;
using UseCase.MedicalExamination.CheckedAfter327Screen;

namespace Interactor.MedicalExamination
{
    public class CheckedAfter327ScreenInteractor : ICheckedAfter327ScreenInputPort
    {
        private readonly IMedicalExaminationRepository _medicalExaminationRepository;
        public CheckedAfter327ScreenInteractor(IMedicalExaminationRepository medicalExaminationRepository)
        {
            _medicalExaminationRepository = medicalExaminationRepository;
        }

        public CheckedAfter327ScreenOutputData Handle(CheckedAfter327ScreenInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new CheckedAfter327ScreenOutputData(CheckedAfter327ScreenStatus.InvalidHpId, string.Empty, new());
                }
                if (inputData.PtId <= 0)
                {
                    return new CheckedAfter327ScreenOutputData(CheckedAfter327ScreenStatus.InvalidPtId, string.Empty, new());
                }
                if (inputData.SinDate < 0)
                {
                    return new CheckedAfter327ScreenOutputData(CheckedAfter327ScreenStatus.InvalidSinDate, string.Empty, new());
                }

                var data = _medicalExaminationRepository.GetCheckedAfter327Screen(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.CheckedOrderModels, inputData.IsTokysyoOrder, inputData.IsTokysyosenOrder);

                return new CheckedAfter327ScreenOutputData(CheckedAfter327ScreenStatus.Successed, data.Item1, data.Item2);
            }
            finally
            {
                _medicalExaminationRepository.ReleaseResource();
            }
        }
    }
}
