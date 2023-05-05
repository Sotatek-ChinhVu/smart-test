using UseCase.Core.Sync.Core;

namespace UseCase.MainMenu.GetStatisticMenu;

public class GetStatisticMenuOutputData : IOutputData
{
    public GetStatisticMenuOutputData(List<StatisticMenuItem> statisticMenuList, List<StaGrpItem> staGrpItemList, GetStatisticMenuStatus status)
    {
        StatisticMenuList = statisticMenuList;
        Status = status;
        StaGrpItemList = staGrpItemList;
    }

    public GetStatisticMenuOutputData(GetStatisticMenuStatus status)
    {
        StatisticMenuList = new();
        StaGrpItemList = new();
        Status = status;
    }

    public List<StaGrpItem> StaGrpItemList { get; private set; }

    public List<StatisticMenuItem> StatisticMenuList { get; private set; }

    public GetStatisticMenuStatus Status { get; private set; }
}
