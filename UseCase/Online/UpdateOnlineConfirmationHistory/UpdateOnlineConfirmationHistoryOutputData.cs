using UseCase.Core.Sync.Core;

namespace UseCase.Online.UpdateOnlineConfirmationHistory;

public class UpdateOnlineConfirmationHistoryOutputData : IOutputData
{
    public UpdateOnlineConfirmationHistoryOutputData(UpdateOnlineConfirmationHistoryStatus status, bool updateSuccessed)
    {
        Status = status;
        UpdateSuccessed = updateSuccessed;
    }

    public UpdateOnlineConfirmationHistoryStatus Status { get; private set; }

    public bool UpdateSuccessed { get; private set; }
}
