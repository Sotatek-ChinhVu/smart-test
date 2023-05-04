using UseCase.Core.Sync.Core;

namespace UseCase.MainMenu.SaveStatisticMenu;

public class SaveStatisticMenuOutputData : IOutputData
{
    public SaveStatisticMenuOutputData(SaveStatisticMenuStatus status)
    {
        Status = status;
    }

    public SaveStatisticMenuStatus Status { get; private set; }
}
