using UseCase.Core.Sync.Core;

namespace UseCase.Online.UpdateOnlineConsents;

public class UpdateOnlineConsentsOutputData : IOutputData
{
    public UpdateOnlineConsentsOutputData(UpdateOnlineConsentsStatus status)
    {
        Status = status;
    }

    public UpdateOnlineConsentsStatus Status { get; private set; }
}
