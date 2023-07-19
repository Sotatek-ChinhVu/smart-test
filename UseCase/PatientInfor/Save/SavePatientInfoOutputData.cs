using Domain.Models.PatientInfor;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.Save
{
    public class SavePatientInfoOutputData : IOutputData
    {
        public IEnumerable<SavePatientInfoValidationResult> ValidateDetails { get; private set; }

        public SavePatientInfoStatus Status { get; private set; }

        public long PtID { get; private set; }

        public PatientInforModel PatientInforModel { get; private set; }

        public bool ShouldCheckCloneByomei { get; private set; }

        public SavePatientInfoOutputData(IEnumerable<SavePatientInfoValidationResult> validateDetails, SavePatientInfoStatus status, long ptID, PatientInforModel patientInforModel, bool shouldCheckCloneByomei)
        {
            ValidateDetails = validateDetails;
            Status = status;
            PtID = ptID;
            PatientInforModel = patientInforModel;
            ShouldCheckCloneByomei = shouldCheckCloneByomei;
        }
    }
}
