using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.Save
{
    public class SavePatientInfoOutputData : IOutputData
    {
        public string Message { get; private set; }
        public SavePatientInfoStatus Status { get; private set; }
        public long PtID { get; private set; }
        public SavePatientInfoOutputData(string message, SavePatientInfoStatus status, long ptID)
        {
            Message = message;
            Status = status;
            PtID = ptID;
        }
    }
}
