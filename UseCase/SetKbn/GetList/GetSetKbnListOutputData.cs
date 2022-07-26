using UseCase.Core.Sync.Core;

namespace UseCase.SetKbn.GetList
{
    public class GetSetKbnListOutputData : IOutputData
    {
        public List<GetSetKbnListOutputItem>? SetList { get; private set; }
        public GetSetKbnListStatus Status { get; private set; }
        public GetSetKbnListOutputData(List<GetSetKbnListOutputItem>? setList, GetSetKbnListStatus status)
        {
            SetList = setList;
            Status = status;
        }
    }
}