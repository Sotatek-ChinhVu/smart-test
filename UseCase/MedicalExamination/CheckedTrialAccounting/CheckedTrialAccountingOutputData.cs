using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.CheckedTrialAccounting
{
    public class CheckedTrialAccountingOutputData : IOutputData
    {
        public CheckedTrialAccountingOutputData(string message, CheckedTrialAccountingStatus status)
        {
            Message = message;
            Status = status;
        }

        public string Message { get; private set; }
        public CheckedTrialAccountingStatus Status { get; private set; }
    }
}
