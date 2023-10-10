using UseCase.Core.Sync.Core;

namespace UseCase.Online.SaveOnlineConfirmation
{
    public class SaveOnlineConfirmationOutputData : IOutputData
    {
        public SaveOnlineConfirmationOutputData(SaveOnlineConfirmationStatus status, string message)
        {
            Status = status;
            Message = message;
        }

        public SaveOnlineConfirmationStatus Status { get; private set; }

        public string Message { get; private set; }
    }
}
