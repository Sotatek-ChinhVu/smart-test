using Domain.Models.RaiinFilterMst;
using UseCase.Core.Sync.Core;

namespace UseCase.RaiinFilterMst.GetList;

public class GetRaiinFilterMstListOutputData : IOutputData
{
    public GetRaiinFilterMstListOutputData(GetRaiinFilterMstListStatus status, List<RaiinFilterMstModel> filters)
    {
        Status = status;
        FilterMsts = filters;
    }

    public GetRaiinFilterMstListStatus Status { get; private set; }
    public List<RaiinFilterMstModel> FilterMsts { get; private set; } = new List<RaiinFilterMstModel>();
}
