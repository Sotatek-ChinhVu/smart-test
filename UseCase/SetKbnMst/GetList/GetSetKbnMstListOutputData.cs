using UseCase.Core.Sync.Core;

namespace UseCase.SetKbnMst.GetList
{
    public class GetSetKbnMstListOutputData : IOutputData
    {
        public List<GetSetKbnMstListOutputItem>? SetList { get; private set; }
        public GetSetKbnMstListStatus Status { get; private set; }
        public GetSetKbnMstListOutputData(List<GetSetKbnMstListOutputItem>? setList, GetSetKbnMstListStatus status)
        {
            SetList = setList;
            Status = status;
        }
    }
}