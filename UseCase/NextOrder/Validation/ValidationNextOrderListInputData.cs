using UseCase.Core.Sync.Core;

namespace UseCase.NextOrder.Validation
{
    public class ValidationNextOrderListInputData : IInputData<ValidationNextOrderListOutputData>
    {
        public long PtId { get; private set; }

        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public List<NextOrderItem> NextOrderItems { get; private set; }

        public ValidationNextOrderListInputData(long ptId, int hpId, int userId, List<NextOrderItem> nextOrderItems)
        {
            PtId = ptId;
            HpId = hpId;
            UserId = userId;
            NextOrderItems = nextOrderItems;
        }
    }
}
