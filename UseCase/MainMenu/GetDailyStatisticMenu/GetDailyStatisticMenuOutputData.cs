using UseCase.Core.Sync.Core;

namespace UseCase.MainMenu.GetDailyStatisticMenu;

public class GetDailyStatisticMenuOutputData : IOutputData
{
    public GetDailyStatisticMenuOutputData(List<StatisticMenuItem> statisticMenuList, List<StaGrpItem> staGrpItemList, GetDailyStatisticMenuStatus status)
    {
        StatisticMenuList = statisticMenuList;
        Status = status;
        StaGrpItemList = staGrpItemList;
    }

    public GetDailyStatisticMenuOutputData(GetDailyStatisticMenuStatus status)
    {
        StatisticMenuList = new();
        StaGrpItemList = new();
        Status = status;
    }

    public List<StaGrpItem> StaGrpItemList { get; private set; }

    public List<StatisticMenuItem> StatisticMenuList { get; private set; }

    public GetDailyStatisticMenuStatus Status { get; private set; }
}
