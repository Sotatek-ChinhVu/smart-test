using UseCase.Core.Sync.Core;

namespace UseCase.MainMenu.GetDailyStatisticMenu;

public class GetDailyStatisticMenuOutputData : IOutputData
{
    public GetDailyStatisticMenuOutputData(List<StatisticMenuItem> statisticMenuList, GetDailyStatisticMenuStatus status)
    {
        StatisticMenuList = statisticMenuList;
        Status = status;
    }

    public GetDailyStatisticMenuOutputData(GetDailyStatisticMenuStatus status)
    {
        StatisticMenuList = new();
        Status = status;
    }

    public List<StatisticMenuItem> StatisticMenuList { get; private set; }

    public GetDailyStatisticMenuStatus Status { get; private set; }
}
