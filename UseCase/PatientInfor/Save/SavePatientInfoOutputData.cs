using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.Save
{
    public class SavePatientInfoOutputData : IOutputData
    {
        public IEnumerable<SavePatientInfoValidationResult> ValidateDetails { get; private set; }

        public SavePatientInfoStatus Status { get; private set; }

        public long PtID { get; private set; }

        public SavePatientInfoOutputData(IEnumerable<SavePatientInfoValidationResult> validateDetails, SavePatientInfoStatus status, long ptID)
        {
            ValidateDetails = validateDetails;
            Status = status;
            PtID = ptID;
        }
    }
}
