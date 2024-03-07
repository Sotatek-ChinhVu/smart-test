using UseCase.Core.Sync.Core;

namespace UseCase.RaiinFilterMst.GetList;

public class GetRaiinFilterMstListInputData : IInputData<GetRaiinFilterMstListOutputData>
{
    public GetRaiinFilterMstListInputData(int hpId)
    {
        HpId = hpId;
    }
    public int HpId { get; private set; }
}
