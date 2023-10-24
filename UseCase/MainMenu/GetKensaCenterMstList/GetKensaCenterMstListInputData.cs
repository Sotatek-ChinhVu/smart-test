using UseCase.Core.Sync.Core;

namespace UseCase.MainMenu.GetKensaCenterMstList;

public class GetKensaCenterMstListInputData : IInputData<GetKensaCenterMstListOutputData>
{
    public GetKensaCenterMstListInputData(int hpId)
    {
        HpId = hpId;
    }

    public int HpId { get; private set; }
}
