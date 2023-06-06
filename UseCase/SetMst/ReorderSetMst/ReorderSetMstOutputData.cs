using UseCase.Core.Sync.Core;
using UseCase.SetMst.GetList;

namespace UseCase.SetMst.ReorderSetMst;

public class ReorderSetMstOutputData : IOutputData
{
    public List<GetSetMstListOutputItem>? setMstModels { get; set; }
    public ReorderSetMstStatus Status { get; private set; }

    public ReorderSetMstOutputData(ReorderSetMstStatus status)
    {
        Status = status;
        setMstModels = new();
    }

    public ReorderSetMstOutputData(List<GetSetMstListOutputItem>? setMstModels, ReorderSetMstStatus status)
    {
        this.setMstModels = setMstModels;
        Status = status;
    }
}
