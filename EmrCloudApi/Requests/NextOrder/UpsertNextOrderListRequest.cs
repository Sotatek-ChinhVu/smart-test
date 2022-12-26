using UseCase.NextOrder;

namespace EmrCloudApi.Requests.NextOrder
{
    public class UpsertNextOrderListRequest
    {
        public long PtId { get; set; }

        public List<NextOrderItem> NextOrderItems { get; set; } = new();

        public FileItemRequestItem FileItem { get; set; } = new();
    }
}
