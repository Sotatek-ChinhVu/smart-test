using UseCase.NextOrder;

namespace EmrCloudApi.Requests.NextOrder
{
    public class ValidationNextOrderListRequest
    {
        public long PtId { get; set; }

        public List<NextOrderItem> NextOrderItems { get; set; } = new();
    }
}
