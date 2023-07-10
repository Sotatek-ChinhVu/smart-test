using UseCase.Core.Sync.Core;

namespace UseCase.SetMst.ReorderSetMst;

public class ReorderSetMstInputData : IInputData<ReorderSetMstOutputData>
{
    public ReorderSetMstInputData(int hpId, long ptId, long raiinNo, int sinDate, int dragSetCd, int dropSetCd, int userId)
    {
        HpId = hpId;
        PtId = ptId;
        RaiinNo = raiinNo;
        SinDate = sinDate;
        DragSetCd = dragSetCd;
        DropSetCd = dropSetCd;
        UserId = userId;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }

    public long RaiinNo { get; private set; }

    public int SinDate { get; private set; }

    public int DragSetCd { get; private set; }

    public int DropSetCd { get; private set; }

    public int UserId { get; private set; }
}
