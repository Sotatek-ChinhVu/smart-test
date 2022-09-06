using UseCase.Core.Sync.Core;

namespace UseCase.SetMst.CopyPasteSetMst;

public class CopyPasteSetMstOutputData : IOutputData
{
    public CopyPasteSetMstStatus Status { get; private set; }
    public CopyPasteSetMstOutputData(CopyPasteSetMstStatus status)
    {
        Status = status;
    }
}
