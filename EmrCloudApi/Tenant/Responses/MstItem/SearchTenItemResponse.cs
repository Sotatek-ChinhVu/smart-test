using Domain.Models.MstItem;

namespace EmrCloudApi.Tenant.Responses.MstItem
{
    public class SearchTenItemResponse
    {
        public SearchTenItemResponse(List<TenItemModel> listData)
        {
            ListData = listData;
        }

        public List<TenItemModel> ListData { get; private set; }

    }
}
