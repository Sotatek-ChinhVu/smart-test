using UseCase.Core.Sync.Core;

namespace UseCase.MainMenu.GetDailyStatisticMenu;

public class GetDailyStatisticMenuInputData : IInputData<GetDailyStatisticMenuOutputData>
{
    public GetDailyStatisticMenuInputData(int hpId, int menuId)
    {
        HpId = hpId;
        MenuId = menuId;
    }

    public int HpId { get; private set; }

    public int MenuId { get; private set; }
}
