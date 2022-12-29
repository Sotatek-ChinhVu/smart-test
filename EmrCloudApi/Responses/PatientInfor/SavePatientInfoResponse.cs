using UseCase.PatientInfor.Save;

namespace EmrCloudApi.Responses.PatientInfor
{
    public class SavePatientInfoResponse
    {
        public SavePatientInfoResponse(IEnumerable<SavePatientInfoValidationResult> validateDetails, SavePatientInfoStatus state, long ptID)
        {
            ValidateDetails = validateDetails;
            State = state;
            PtID = ptID;
        }

        public IEnumerable<SavePatientInfoValidationResult> ValidateDetails { get; private set; }

        public SavePatientInfoStatus State { get; private set; }

        public long PtID { get; private set; }
    }
}