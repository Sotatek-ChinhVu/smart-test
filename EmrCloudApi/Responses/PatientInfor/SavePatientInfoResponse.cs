using UseCase.PatientInfor.Save;

namespace EmrCloudApi.Responses.PatientInfor
{
    public class SavePatientInfoResponse
    {
        public SavePatientInfoResponse(IEnumerable<SavePatientInfoValidationResult> validateDetails, SavePatientInfoStatus state, long ptID, bool shouldCheckCloneByomei)
        {
            ValidateDetails = validateDetails;
            State = state;
            PtID = ptID;
            ShouldCheckCloneByomei = shouldCheckCloneByomei;
        }

        public SavePatientInfoResponse()
        {
            ValidateDetails = Enumerable.Empty<SavePatientInfoValidationResult>(); ;
        }

        public IEnumerable<SavePatientInfoValidationResult> ValidateDetails { get; private set; }

        public SavePatientInfoStatus State { get; private set; }

        public long PtID { get; private set; }

        public bool ShouldCheckCloneByomei { get; private set; }
    }
}