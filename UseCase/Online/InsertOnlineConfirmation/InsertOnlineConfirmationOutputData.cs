using UseCase.Core.Sync.Core;

namespace UseCase.Online.InsertOnlineConfirmation
{
    public class InsertOnlineConfirmationOutputData : IOutputData
    {
        public InsertOnlineConfirmationOutputData(string message, InsertOnlineConfirmationStatus status)
        {
            Message = message;
            Status = status;
        }

        public string Message { get; private set; }

        public InsertOnlineConfirmationStatus Status { get; private set; }
    }
}
