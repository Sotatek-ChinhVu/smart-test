using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.CheckAccountingStatus
{
    public class CheckAccountingStatusOutputData : IOutputData
    {
        public CheckAccountingStatusOutputData(string errorType, string message, CheckAccountingStatus status)
        {
            ErrorType = errorType;
            Message = message;
            Status = status;
        }

        public string ErrorType { get; set; }
        public string Message { get; private set; }
        public CheckAccountingStatus Status { get; private set; }
    }
}
