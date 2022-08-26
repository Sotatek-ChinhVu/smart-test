using Domain.Models.UsageTreeSet;
using UseCase.Core.Sync.Core;

namespace UseCase.UsageTreeSet.GetTree
{
    public class GetUsageTreeSetOutputData : IOutputData
    {
        public List<ListSetMstModel>? ListUsageTreeSet { get; private set; }
        public GetUsageTreeStatus Status { get; private set; }

        public GetUsageTreeSetOutputData(List<ListSetMstModel>? listUsageTreeSet, GetUsageTreeStatus status)
        {
            ListUsageTreeSet = listUsageTreeSet;
            Status = status;
        }
    }
}