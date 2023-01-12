using Domain.Models.NextOrder;
using UseCase.NextOrder;

namespace EmrCloudApi.Responses.NextOrder
{
    public class GetNextOrderListResponse
    {
        public GetNextOrderListResponse(List<NextOrderLabelItem> nextOrders)
        {
            NextOrders = nextOrders;
        }

        public List<NextOrderLabelItem> NextOrders { get; private set; }
    }
}
