using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetByomeiByCode
{
    public sealed class GetByomeiByCodeOutputData : IOutputData
    {
        public GetByomeiByCodeOutputData(ByomeiMstItem items, GetByomeiByCodeStatus status)
        {
            Items = items;
            Status = status;
        }
        public ByomeiMstItem Items { get; private set; }
        public GetByomeiByCodeStatus Status { get; private set; }
    }
}
