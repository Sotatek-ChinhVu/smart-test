
using Domain.Models.ListSetMst;
using UseCase.Core.Sync.Core;

namespace UseCase.ListSetMst.GetTreeListSet
{
    public sealed class GetTreeListSetOutputData : IOutputData
    {
        public List<ListSetMstModel> ListTreeSet { get; private set; }
        public GetTreeListSetStatus Status { get; private set; }

        public GetTreeListSetOutputData(List<ListSetMstModel> listTreeSet, GetTreeListSetStatus status)
        {
            ListTreeSet = listTreeSet;
            Status = status;
        }
    }
}
