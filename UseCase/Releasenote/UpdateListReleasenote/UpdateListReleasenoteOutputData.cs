using UseCase.Core.Sync.Core;

namespace UseCase.Releasenote.UpdateListReleasenote;

public class UpdateListReleasenoteOutputData : IOutputData
{
    public UpdateListReleasenoteOutputData(UpdateListReleasenoteStatus status)
    {
        Status = status;
    }

    public UpdateListReleasenoteStatus Status { get; private set; }
}
