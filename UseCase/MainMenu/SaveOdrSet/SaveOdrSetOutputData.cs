using UseCase.Core.Sync.Core;

namespace UseCase.MainMenu.SaveOdrSet;

public class SaveOdrSetOutputData : IOutputData
{
    public SaveOdrSetOutputData(SaveOdrSetStatus status)
    {
        Status = status;
    }

    public SaveOdrSetStatus Status { get; private set; }
}
