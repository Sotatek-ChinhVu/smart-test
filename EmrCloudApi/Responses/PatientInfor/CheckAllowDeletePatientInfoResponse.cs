using UseCase.PatientInfor.CheckAllowDeletePatientInfo;

namespace EmrCloudApi.Responses.PatientInfor
{
    public class CheckAllowDeletePatientInfoResponse
    {
        public CheckAllowDeletePatientInfoResponse(CheckAllowDeletePatientInfoStatus status)
        {
            Status = status;
        }

        public CheckAllowDeletePatientInfoStatus Status { get; private set; }
    }
}
