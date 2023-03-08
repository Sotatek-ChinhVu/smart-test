using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.SaveReceCheckOpt;

public class SaveReceCheckOptOutputData : IOutputData
{
    public SaveReceCheckOptOutputData(SaveReceCheckOptStatus status)
    {
        Status = status;
    }

    public SaveReceCheckOptStatus Status { get; private set; }
}
