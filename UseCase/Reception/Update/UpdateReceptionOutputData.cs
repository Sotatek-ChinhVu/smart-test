using UseCase.Core.Sync.Core;

namespace UseCase.Reception.Update;

public class UpdateReceptionOutputData : IOutputData
{
    public UpdateReceptionOutputData(UpdateReceptionStatus status)
    {
        Status = status;
    }

    public UpdateReceptionStatus Status { get; private set; }
}
