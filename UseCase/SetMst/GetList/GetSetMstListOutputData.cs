using UseCase.Core.Sync.Core;

namespace UseCase.SetMst.GetList
{
    public class GetSetMstListOutputData : IOutputData
    {
        public List<GetSetMstListOutputItem>? SetList { get; private set; }
        public GetSetMstListStatus Status { get; private set; }
        public GetSetMstListOutputData(List<GetSetMstListOutputItem>? setList, GetSetMstListStatus status)
        {
            SetList = setList;
            Status = status;
        }
    }
}