using Domain.Models.MedicalExamination;
using UseCase.ContainerMasterUpdate;

namespace Interactor.MedicalExamination
{
    public class ContainerMasterUpdateInteractor : IContainerMasterUpdateInputPort
    {
        private readonly IMedicalExaminationRepository _medicalExaminationRepository;
        public ContainerMasterUpdateInteractor(IMedicalExaminationRepository medicalExaminationRepository) 
        {
            _medicalExaminationRepository = medicalExaminationRepository;
        }
        public ContainerMasterUpdateOutPutData Handle(ContainerMasterUpdateInputData inputData)
        {
            try
            {
                _medicalExaminationRepository.ContainerMasterUpdate(inputData.HpId, inputData.UserId, inputData.ContainerMasters);
                return new ContainerMasterUpdateOutPutData(ContainerMasterUpdateStatus.Successful);
            }
            finally 
            {
                _medicalExaminationRepository.ReleaseResource();
            }
        }
    }
}
