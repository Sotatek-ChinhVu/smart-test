using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetTreeByomeiSet
{
    public sealed class GetTreeByomeiSetOutputData : IOutputData
    {
        public IEnumerable<ByomeiSetMstItem> Datas { get; private set; }

        public GetTreeByomeiSetStatus Status { get; private set; }

        public GetTreeByomeiSetOutputData(IEnumerable<ByomeiSetMstItem> datas, GetTreeByomeiSetStatus status)
        {
            Datas = datas;
            Status = status;
        }
    }
}
