using UseCase.Core.Sync.Core;

namespace UseCase.SetMst.GetListSetGenerationMst;

public class GetSetGenerationMstListInputData : IInputData<GetSetGenerationMstListOutputData>
{
    public GetSetGenerationMstListInputData(int hpId)
    {
        HpId = hpId;
    }

    public int HpId { get; private set; }
}
