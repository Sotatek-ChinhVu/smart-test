using Domain.Models.MstItem;

namespace EmrCloudApi.Tenant.Responses.MstItem
{
    public class SearchSupplementResponse
    {
        public SearchSupplementResponse(List<SearchSupplementBaseModel> listData, int total)
        {
            ListData = listData;
            Total = total;
        }

        public List<SearchSupplementBaseModel> ListData { get; private set; }
        public int Total { get; set; }
    }
}
