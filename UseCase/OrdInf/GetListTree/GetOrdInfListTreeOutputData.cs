using UseCase.Core.Sync.Core;

namespace UseCase.OrdInfs.GetListTrees
{
    public class GetOrdInfListTreeOutputData : IOutputData
    {

        public List<GroupHokenItem> GroupHokens { get; private set; }
        public GetOrdInfListTreeStatus Status { get; private set; }

        public GetOrdInfListTreeOutputData(List<GroupHokenItem> groupHokens, GetOrdInfListTreeStatus status)
        {
            GroupHokens = groupHokens;
            Status = status;
        }
    }
}
