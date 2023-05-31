using Domain.Models.SetMst;
using UseCase.Core.Sync.Core;

namespace UseCase.SetMst.ReorderSetMst;

public class ReorderSetMstOutputData : IOutputData
{
    public List<SetMstModel> setMstModels { get; set; }
    public ReorderSetMstStatus Status { get; private set; }

    public ReorderSetMstOutputData(ReorderSetMstStatus status)
    {
        Status = status;
        setMstModels = new List<SetMstModel>();
    }

    public ReorderSetMstOutputData(List<SetMstModel> setMstModels, ReorderSetMstStatus status)
    {
        this.setMstModels = setMstModels;
        Status = status;
    }
}
