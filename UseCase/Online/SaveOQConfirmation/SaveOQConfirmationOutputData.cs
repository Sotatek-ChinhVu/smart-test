using UseCase.Core.Sync.Core;

namespace UseCase.Online.SaveOQConfirmation;

public class SaveOQConfirmationOutputData : IOutputData
{
    public SaveOQConfirmationOutputData(SaveOQConfirmationStatus status)
    {
        Status = status;
    }

    public SaveOQConfirmationStatus Status { get; private set; }
}
