using Domain.Models.MedicalExamination;
using UseCase.UpsertMaterialMaster;

namespace Interactor.MedicalExamination
{
    public class UpsertMaterialMasterInteractor : IUpsertMaterialMasterInputPort
    {
        private readonly IMedicalExaminationRepository _medicalExaminationRepository;
        public UpsertMaterialMasterInteractor(IMedicalExaminationRepository medicalExaminationRepository)
        {
            _medicalExaminationRepository = medicalExaminationRepository;
        }
        public UpsertMaterialMasterOutputData Handle(UpsertMaterialMasterInputData inputData)
        {
            try
            {
                _medicalExaminationRepository.UpsertMaterialMaster(inputData.HpId, inputData.UserId, inputData.MaterialMasters);
                return new UpsertMaterialMasterOutputData(UpsertMaterialMasterStatus.Success);
            }
            finally
            {
                _medicalExaminationRepository.ReleaseResource();
            }
        }
    }
}
