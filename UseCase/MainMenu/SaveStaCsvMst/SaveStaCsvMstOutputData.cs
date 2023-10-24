using UseCase.Core.Sync.Core;

namespace UseCase.MainMenu.SaveStaCsvMst;

public class SaveStaCsvMstOutputData : IOutputData
{
    public SaveStaCsvMstOutputData(SaveStaCsvMstStatus status)
    {
        Status = status;
    }

    public SaveStaCsvMstStatus Status { get; private set; }
}
