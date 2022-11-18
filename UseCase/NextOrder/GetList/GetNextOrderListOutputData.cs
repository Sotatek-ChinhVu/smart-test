using Domain.Models.NextOrder;
using UseCase.Core.Sync.Core;

namespace UseCase.NextOrder.GetList
{
    public class GetNextOrderListOutputData : IOutputData
    {

        public List<NextOrderModel> NextOrders { get; private set; }
        public GetNextOrderListStatus Status { get; private set; }

        public GetNextOrderListOutputData(List<NextOrderModel> nextOrders, GetNextOrderListStatus status)
        {
            NextOrders = nextOrders;
            Status = status;
        }
    }
}
