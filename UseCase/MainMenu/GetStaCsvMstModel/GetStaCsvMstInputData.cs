using UseCase.Core.Sync.Core;

namespace UseCase.MainMenu.GetStaCsvMstModel;

public class GetStaCsvMstInputData : IInputData<GetStaCsvMstOutputData>
{
    public GetStaCsvMstInputData(int hpId)
    {
        HpId = hpId;
    }

    public int HpId { get; private set; }
}
