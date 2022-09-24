using Domain.Models.MstItem;

namespace EmrCloudApi.Tenant.Responses.MstItem
{
    public class SearchTenItemResponse
    {
        public SearchTenItemResponse(List<TenItemModel> listData, int totalCount)
        {
            ListData = listData;
            TotalCount = totalCount;
        }
        public int TotalCount { get; private set; }
        public List<TenItemModel> ListData { get; private set; }

    }
}
