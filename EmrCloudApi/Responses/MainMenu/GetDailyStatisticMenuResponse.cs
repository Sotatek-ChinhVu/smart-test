using EmrCloudApi.Responses.MainMenu.Dto;
using UseCase.MainMenu;

namespace EmrCloudApi.Responses.MainMenu;

public class GetDailyStatisticMenuResponse
{
    public GetDailyStatisticMenuResponse(List<StatisticMenuItem> statisticMenuList)
    {
        StatisticMenuList = statisticMenuList.Select(item => new StatisticMenuDto(item)).ToList();
    }

    public List<StatisticMenuDto> StatisticMenuList { get; private set; }
}
