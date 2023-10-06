using UseCase.Core.Sync.Core;

namespace UseCase.Online.UpdateOQConfirmation;

public class UpdateOQConfirmationOutputData : IOutputData
{
    public UpdateOQConfirmationOutputData(UpdateOQConfirmationStatus status)
    {
        Status = status;
    }

    public UpdateOQConfirmationStatus Status { get; private set; }
}
