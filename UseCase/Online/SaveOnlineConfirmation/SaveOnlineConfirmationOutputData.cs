using UseCase.Core.Sync.Core;

namespace UseCase.Online.SaveOnlineConfirmation
{
    public class SaveOnlineConfirmationOutputData : IOutputData
    {
        public SaveOnlineConfirmationOutputData(SaveOnlineConfirmationStatus status)
        {
            Status = status;
        }

        public SaveOnlineConfirmationStatus Status { get; private set; }
    }
}
