using UseCase.Core.Sync.Core;

namespace UseCase.Online.SaveOnlineConfirmation
{
    public class UpdateOnlineConfirmationOutputData : IOutputData
    {
        public UpdateOnlineConfirmationOutputData(UpdateOnlineConfirmationStatus status, string message)
        {
            Status = status;
            Message = message;
        }

        public UpdateOnlineConfirmationStatus Status { get; private set; }

        public string Message { get; private set; }
    }
}
