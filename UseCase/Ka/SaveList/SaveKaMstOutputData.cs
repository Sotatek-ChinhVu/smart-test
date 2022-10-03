using UseCase.Core.Sync.Core;

namespace UseCase.Ka.SaveList;

public class SaveKaMstOutputData : IOutputData
{
    public SaveKaMstStatus Status { get; private set; }

    public SaveKaMstOutputData(SaveKaMstStatus status)
    {
        Status = status;
    }
}
