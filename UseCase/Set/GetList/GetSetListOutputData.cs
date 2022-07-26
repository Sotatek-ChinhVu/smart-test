using UseCase.Core.Sync.Core;

namespace UseCase.Set.GetList
{
    public class GetSetListOutputData : IOutputData
    {
        public List<GetSetListOutputItem>? SetList { get; private set; }
        public GetSetListStatus Status { get; private set; }
        public GetSetListOutputData(List<GetSetListOutputItem>? setList, GetSetListStatus status)
        {
            SetList = setList;
            Status = status;
        }
    }
}