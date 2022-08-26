using UseCase.Core.Sync.Core;

namespace UseCase.SetMst.ReorderSetMst;

public class ReorderSetMstOutputData : IOutputData
{
    public ReorderSetMstStatus Status { get; private set; }
    public ReorderSetMstOutputData(ReorderSetMstStatus status)
    {
        Status = status;
    }
}
