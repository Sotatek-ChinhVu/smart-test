using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.CheckedExpired
{
    public class CheckedExpiredOutputData : IOutputData
    {
        public CheckedExpiredOutputData(CheckedExpiredStatus status, List<string> messages)
        {
            Status = status;
            Messages = messages;
        }

        public CheckedExpiredStatus Status { get; private set; }
        public List<string> Messages { get; private set; }
    }
}
