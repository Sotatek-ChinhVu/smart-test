using UseCase.Core.Sync.Core;

namespace UseCase.NextOrder.Upsert
{
    public class UpsertNextOrderListInputData : IInputData<UpsertNextOrderListOutputData>
    {
        public long PtId { get; private set; }

        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public List<NextOrderItem> NextOrderItems { get; private set; }

        public UpsertNextOrderListInputData(long ptId, int hpId, int userId, List<NextOrderItem> nextOrderItems)
        {
            PtId = ptId;
            HpId = hpId;
            UserId = userId;
            NextOrderItems = nextOrderItems;
        }
    }
}
