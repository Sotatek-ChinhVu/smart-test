using UseCase.PatientInfor.CheckValidSamePatient;

namespace EmrCloudApi.Responses.PatientInfor
{
    public class CheckValidSamePatientResponse
    {
        public CheckValidSamePatientResponse(CheckValidSamePatientStatus status)
        {
            Status = status;
        }

        public CheckValidSamePatientStatus Status { get; private set; }
    }
}
