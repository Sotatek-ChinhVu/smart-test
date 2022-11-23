using UseCase.NextOrder.Upsert;

namespace EmrCloudApi.Tenant.Requests.NextOrder
{
    public class UpsertNextOrderListRequest
    {
        public long PtId { get; set; }

        public List<NextOrderItem> NextOrderItems { get; set; } = new();
    }
}
