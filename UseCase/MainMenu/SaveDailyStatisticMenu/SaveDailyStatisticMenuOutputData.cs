using UseCase.Core.Sync.Core;

namespace UseCase.MainMenu.SaveDailyStatisticMenu;

public class SaveDailyStatisticMenuOutputData : IOutputData
{
    public SaveDailyStatisticMenuOutputData(SaveDailyStatisticMenuStatus status)
    {
        Status = status;
    }

    public SaveDailyStatisticMenuStatus Status { get; private set; }
}
