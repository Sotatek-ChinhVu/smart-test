using Domain.Constant;
using Domain.Models.PatientInfor;
using UseCase.PatientInfor.CheckAllowDeletePatientInfo;

namespace Interactor.PatientInfor
{
    public class CheckAllowDeletePatientInfoInteractor : ICheckAllowDeletePatientInfoInputPort
    {
        private readonly IPatientInforRepository _patientInforRepository;
        public CheckAllowDeletePatientInfoInteractor(IPatientInforRepository patientInforRepository)
        {
            _patientInforRepository = patientInforRepository;
        }

        public CheckAllowDeletePatientInfoOutputData Handle(CheckAllowDeletePatientInfoInputData inputData)
        {
            if (inputData.PtId < 0)
                return new CheckAllowDeletePatientInfoOutputData(CheckAllowDeletePatientInfoStatus.InvalidPtId, string.Empty);

            if (inputData.HpId < 0)
                return new CheckAllowDeletePatientInfoOutputData(CheckAllowDeletePatientInfoStatus.InvalidHpId, string.Empty);

            try
            {
                if (!_patientInforRepository.IsAllowDeletePatient(inputData.HpId, inputData.PtId))
                {
                    string message = string.Format(ErrorMessage.MessageType_mDel01060, "入力された情報がある", "患者情報");
                    return new CheckAllowDeletePatientInfoOutputData(CheckAllowDeletePatientInfoStatus.NotAllowDelete, message);
                }
                else
                {
                    return new CheckAllowDeletePatientInfoOutputData(CheckAllowDeletePatientInfoStatus.AllowDelete, string.Empty);
                }
                    
            }
            finally
            {
                _patientInforRepository.ReleaseResource();
            }
        }
    }
}
