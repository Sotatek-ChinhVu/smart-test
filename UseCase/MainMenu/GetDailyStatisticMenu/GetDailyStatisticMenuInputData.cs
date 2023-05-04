using UseCase.Core.Sync.Core;

namespace UseCase.MainMenu.GetDailyStatisticMenu;

public class GetDailyStatisticMenuInputData : IInputData<GetDailyStatisticMenuOutputData>
{
    public GetDailyStatisticMenuInputData(int hpId, int grpId)
    {
        HpId = hpId;
        GrpId = grpId;
    }

    public int HpId { get; private set; }

    public int GrpId { get; private set; }
}
