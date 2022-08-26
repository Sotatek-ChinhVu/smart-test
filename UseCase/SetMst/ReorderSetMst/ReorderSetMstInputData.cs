using UseCase.Core.Sync.Core;

namespace UseCase.SetMst.ReorderSetMst;

public class ReorderSetMstInputData : IInputData<ReorderSetMstOutputData>
{
    public ReorderSetMstInputData(int hpId, int dragSetCd, int dropSetCd)
    {
        HpId = hpId;
        DragSetCd = dragSetCd;
        DropSetCd = dropSetCd;
    }

    public int HpId { get; private set; }
    public int DragSetCd { get; private set; }
    public int DropSetCd { get; private set; }
}
