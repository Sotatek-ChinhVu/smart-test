using EmrCloudApi.Responses.MainMenu.Dto;
using UseCase.MainMenu;

namespace EmrCloudApi.Responses.MainMenu;

public class GetDailyStatisticMenuResponse
{
    public GetDailyStatisticMenuResponse(List<StatisticMenuItem> statisticMenuList, List<StaGrpItem> staGrpList)
    {
        StatisticMenuList = statisticMenuList.Select(item => new StatisticMenuDto(item)).ToList();
        StaGrpList = staGrpList.Select(item => new StaGrpDto(item)).ToList();
    }

    public List<StatisticMenuDto> StatisticMenuList { get; private set; }

    public List<StaGrpDto> StaGrpList { get; private set; }
}
