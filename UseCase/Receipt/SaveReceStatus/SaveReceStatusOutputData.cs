using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.SaveReceStatus;

public class SaveReceStatusOutputData : IOutputData
{
    public SaveReceStatusOutputData(SaveReceStatusStatus status)
    {
        Status = status;
    }

    public SaveReceStatusStatus Status { get; private set; }
}
