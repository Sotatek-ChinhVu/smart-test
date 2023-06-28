using UseCase.Core.Sync.Core;

namespace UseCase.NextOrder.GetList
{
    public class GetNextOrderListInputData : IInputData<GetNextOrderListOutputData>
    {
        public GetNextOrderListInputData(long ptId, int hpId, bool isDeleted)
        {
            PtId = ptId;
            HpId = hpId;
            IsDeleted = isDeleted;
        }

        public long PtId { get; private set; }

        public int HpId { get; private set; }

        public bool IsDeleted { get; private set; }
    }
}
