using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.CheckAccountingStatus
{
    public class CheckAccountingStatusOutputData : IOutputData
    {
        public CheckAccountingStatusOutputData(string message, CheckAccountingStatus status)
        {
            Message = message;
            Status = status;
        }

        public string Message { get; private set; }
        public CheckAccountingStatus Status { get; private set; }
    }
}
