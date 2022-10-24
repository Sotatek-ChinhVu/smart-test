using UseCase.PatientInfor.DeletePatient;

namespace EmrCloudApi.Tenant.Responses.PatientInfor
{
    public class DeletePatientInfoResponse
    {
        public DeletePatientInfoResponse(DeletePatientInfoStatus state)
        {
            State = state;
        }

        public DeletePatientInfoStatus State { get; private set; }
    }
}
