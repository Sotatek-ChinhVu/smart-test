using UseCase.Core.Sync.Core;

namespace UseCase.MainMenu.SaveDailyStatisticMenu;

public class SaveDailyStatisticMenuInputData : IInputData<SaveDailyStatisticMenuOutputData>
{
    public SaveDailyStatisticMenuInputData(int hpId, int userId, int grpId, List<StatisticMenuItem> staticMenuList)
    {
        HpId = hpId;
        UserId = userId;
        StaticMenuList = staticMenuList;
        GrpId = grpId;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public int GrpId { get; private set; }

    public List<StatisticMenuItem> StaticMenuList { get; private set; }
}
