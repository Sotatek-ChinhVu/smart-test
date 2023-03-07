using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.Recaculate
{
    public class RecaculationOutputData : IOutputData
    {
        public RecaculationOutputData(string message, RecaculationStatus status)
        {
            Message = message;
            Status = status;
        }

        public string Message { get; private set; }
        public RecaculationStatus Status { get; private set; }
    }
}
