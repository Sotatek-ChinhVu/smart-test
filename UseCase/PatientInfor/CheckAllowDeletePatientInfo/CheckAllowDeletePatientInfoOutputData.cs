using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.CheckAllowDeletePatientInfo
{
    public class CheckAllowDeletePatientInfoOutputData : IOutputData
    {
        public CheckAllowDeletePatientInfoOutputData(CheckAllowDeletePatientInfoStatus status, string message)
        {
            Status = status;
            Message = message;
        }

        public CheckAllowDeletePatientInfoStatus Status { get; private set; }

        public string Message { get; private set; }
    }
}
