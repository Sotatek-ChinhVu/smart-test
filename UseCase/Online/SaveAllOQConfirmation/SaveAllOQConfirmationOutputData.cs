using UseCase.Core.Sync.Core;

namespace UseCase.Online.SaveAllOQConfirmation;

public class SaveAllOQConfirmationOutputData : IOutputData
{
    public SaveAllOQConfirmationOutputData(SaveAllOQConfirmationStatus status)
    {
        Status = status;
    }

    public SaveAllOQConfirmationStatus Status { get; private set; }
}
