using Domain.Models.MstItem;

namespace EmrCloudApi.Tenant.Responses.MstItem
{
    public class SearchSupplementResponse
    {
        public SearchSupplementResponse(List<SearchSupplementModel> listData)
        {
            ListData = listData;
        }

        public List<SearchSupplementModel> ListData { get; private set; }
    }
}
