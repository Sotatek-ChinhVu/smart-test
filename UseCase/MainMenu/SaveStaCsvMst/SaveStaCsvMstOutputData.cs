using UseCase.Core.Sync.Core;

namespace UseCase.MainMenu.SaveStaCsvMst;

public class SaveStaCsvMstOutputData : IOutputData
{
    public SaveStaCsvMstOutputData(int menuIdTemp, SaveStaCsvMstStatus status)
    {
        Status = status;
        MenuIdTemp = menuIdTemp;
    }

    public int MenuIdTemp { get; private set; }

    public SaveStaCsvMstStatus Status { get; private set; }
}
