using UseCase.PatientInfor.Save;

namespace EmrCloudApi.Tenant.Responses.PatientInfor
{
    public class SavePatientInfoResponse
    {
        public SavePatientInfoResponse(SavePatientInfoStatus state, long ptID)
        {
            State = state;
            PtID = ptID;
        }

        public SavePatientInfoStatus State { get; private set; }
        public long PtID { get; private set; }
    }
}