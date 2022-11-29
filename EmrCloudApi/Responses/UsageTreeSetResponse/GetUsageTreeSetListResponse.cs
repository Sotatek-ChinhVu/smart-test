using Domain.Models.UsageTreeSet;

namespace EmrCloudApi.Responses.UsageTreeSetResponse
{
    public class GetUsageTreeSetListResponse
    {
        public List<ListSetMstModel> Data { get; set; } = new List<ListSetMstModel>();
    }
}