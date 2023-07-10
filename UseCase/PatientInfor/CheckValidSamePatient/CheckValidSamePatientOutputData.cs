using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.CheckValidSamePatient
{
    public class CheckValidSamePatientOutputData : IOutputData
    {
        public CheckValidSamePatientOutputData(CheckValidSamePatientStatus status, string message)
        {
            Status = status;
            Message = message;
        }

        public CheckValidSamePatientStatus Status { get; private set; }

        public string Message { get; private set; }
    }
}
