using EmrCloudApi.Requests.MainMenu.RequestItem;

namespace EmrCloudApi.Requests.MainMenu;

public class SaveStatisticMenuRequest
{
    public int GrpId { get; set; }

    public List<StatisticMenuRequestItem> StatisticMenuList { get; set; } = new();
}
