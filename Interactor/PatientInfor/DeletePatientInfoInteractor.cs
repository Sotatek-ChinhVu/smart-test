using Domain.Models.PatientInfor;
using UseCase.PatientInfor.DeletePatient;

namespace Interactor.PatientInfor
{
    public class DeletePatientInfoInteractor : IDeletePatientInfoInputPort
    {
        private readonly IPatientInforRepository _patientInforRepository;
        public DeletePatientInfoInteractor(IPatientInforRepository patientInforRepository)
        {
            _patientInforRepository = patientInforRepository;
        }

        public DeletePatientInfoOutputData Handle(DeletePatientInfoInputData inputData)
        {
            if (inputData.PtId < 0)
                return new DeletePatientInfoOutputData(DeletePatientInfoStatus.InvalidPtId);

            if (inputData.HpId < 0)
                return new DeletePatientInfoOutputData(DeletePatientInfoStatus.InvalidHpId);

            bool result = _patientInforRepository.DeletePatientInfo(inputData.PtId, inputData.HpId);
            if (result)
                return new DeletePatientInfoOutputData(DeletePatientInfoStatus.Successful);
            else
                return new DeletePatientInfoOutputData(DeletePatientInfoStatus.Failed);
        }
    }
}
