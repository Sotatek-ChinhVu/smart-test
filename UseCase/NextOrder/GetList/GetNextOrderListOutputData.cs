using Domain.Models.NextOrder;
using UseCase.Core.Sync.Core;

namespace UseCase.NextOrder.GetList
{
    public class GetNextOrderListOutputData : IOutputData
    {

        public List<NextOrderLabelItem> NextOrders { get; private set; }
        public GetNextOrderListStatus Status { get; private set; }

        public GetNextOrderListOutputData(List<NextOrderLabelItem> nextOrders, GetNextOrderListStatus status)
        {
            NextOrders = nextOrders;
            Status = status;
        }
    }
}
