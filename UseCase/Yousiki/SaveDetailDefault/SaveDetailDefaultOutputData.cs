using UseCase.Core.Sync.Core;

namespace UseCase.Yousiki.SaveDetailDefault;

public class SaveDetailDefaultOutputData : IOutputData
{
    public SaveDetailDefaultOutputData(SaveDetailDefaultStatus status)
    {
        Status = status;
    }

    public SaveDetailDefaultStatus Status { get; private set; }
}
