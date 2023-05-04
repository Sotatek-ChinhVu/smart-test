using UseCase.Core.Sync.Core;

namespace UseCase.MainMenu.GetStatisticMenu;

public class GetStatisticMenuInputData : IInputData<GetStatisticMenuOutputData>
{
    public GetStatisticMenuInputData(int hpId, int grpId)
    {
        HpId = hpId;
        GrpId = grpId;
    }

    public int HpId { get; private set; }

    public int GrpId { get; private set; }
}
