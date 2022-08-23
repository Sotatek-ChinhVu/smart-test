using UseCase.Core.Sync.Core;

namespace UseCase.SetMst.ReorderSetMstList;

public class ReorderSetMstInputData : IInputData<ReorderSetMstOutputData>
{
    public ReorderSetMstInputData(List<ReorderSetMstInputItem> setMstLists)
    {
        SetMstLists = setMstLists;
    }

    public List<ReorderSetMstInputItem> SetMstLists { get; private set; }
}
