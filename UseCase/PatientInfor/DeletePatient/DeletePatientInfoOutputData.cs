using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.DeletePatient
{
    public class DeletePatientInfoOutputData : IOutputData
    {
        public DeletePatientInfoStatus Status { get; private set; }
        public DeletePatientInfoOutputData(DeletePatientInfoStatus status)
        {
            Status = status;
        }
    }
}
