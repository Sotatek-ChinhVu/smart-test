using UseCase.PatientInfor.Save;

namespace EmrCloudApi.Tenant.Responses.PatientInfor
{
    public class SavePatientInfoResponse
    {
        public SavePatientInfoResponse(SavePatientInfoStatus state)
        {
            State = state;
        }

        public SavePatientInfoStatus State { get; private set; }
    }
}