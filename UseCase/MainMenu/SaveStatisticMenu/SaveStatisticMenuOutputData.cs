using UseCase.Core.Sync.Core;

namespace UseCase.MainMenu.SaveStatisticMenu;

public class SaveStatisticMenuOutputData : IOutputData
{
    public SaveStatisticMenuOutputData(int menuIdTemp, SaveStatisticMenuStatus status)
    {
        Status = status;
        MenuIdTemp = menuIdTemp;
    }

    public int MenuIdTemp { get; private set; }

    public SaveStatisticMenuStatus Status { get; private set; }
}
