using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.GetListHokenSelect
{
    public class GetListHokenSelectOutputData : IOutputData
    {
        public GetListHokenSelectOutputData(List<ListHokenSelectDto> hokenSelects, GetListHokenSelectStatus status)
        {
            HokenSelects = hokenSelects;
            Status = status;
        }

        public List<ListHokenSelectDto> HokenSelects { get; private set; }
        public GetListHokenSelectStatus Status { get; private set; }
    }
}