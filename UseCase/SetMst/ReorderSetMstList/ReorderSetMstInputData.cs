using UseCase.Core.Sync.Core;

namespace UseCase.SetMst.ReorderSetMstList;

public class ReorderSetMstInputData : IInputData<ReorderSetMstOutputData>
{
    public ReorderSetMstInputData(ReorderSetMstInputItem dragSetMstItem, ReorderSetMstInputItem dropSetMstItem)
    {
        DragSetMstItem = dragSetMstItem;
        DropSetMstItem = dropSetMstItem;
    }

    public ReorderSetMstInputItem DragSetMstItem { get; private set; }
    public ReorderSetMstInputItem DropSetMstItem { get; private set; }
}
