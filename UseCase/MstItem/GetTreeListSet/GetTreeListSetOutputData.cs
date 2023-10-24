
using Domain.Models.ListSetMst;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetTreeListSet
{
    public sealed class GetTreeListSetOutputData : IOutputData
    {
        public List<ListSetMstModel> TreeListSet { get; private set; }
        public GetTreeListSetStatus Status { get; private set; }

        public GetTreeListSetOutputData(List<ListSetMstModel> treeListSet, GetTreeListSetStatus status)
        {
            TreeListSet = treeListSet;
            Status = status;
        }
    }
}
