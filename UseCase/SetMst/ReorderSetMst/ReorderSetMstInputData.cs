using UseCase.Core.Sync.Core;

namespace UseCase.SetMst.ReorderSetMst;

public class ReorderSetMstInputData : IInputData<ReorderSetMstOutputData>
{
    public ReorderSetMstInputData(int hpId, int dragSetCd, int dropSetCd, int userId)
    {
        HpId = hpId;
        DragSetCd = dragSetCd;
        DropSetCd = dropSetCd;
        UserId = userId;
    }

    public int HpId { get; private set; }
    public int DragSetCd { get; private set; }
    public int DropSetCd { get; private set; }
    public int UserId { get; private set; }
}
