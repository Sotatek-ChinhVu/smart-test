using UseCase.Core.Sync.Core;

namespace UseCase.SetMst.CopyPasteSetMst;

public class CopyPasteSetMstOutputData : IOutputData
{
    public CopyPasteSetMstStatus Status { get; private set; }

    public int NewSetCd { get; private set; }

    public CopyPasteSetMstOutputData(CopyPasteSetMstStatus status)
    {
        Status = status;
    }

    public CopyPasteSetMstOutputData(int newSetCd, CopyPasteSetMstStatus status)
    {
        Status = status;
        NewSetCd = newSetCd;
    }
}
