using Domain.Models.MstItem;

namespace EmrCloudApi.Tenant.Responses.MstItem
{
    public class SearchOTCResponse
    {
        public SearchOTCResponse(List<SearchOTCBaseModel> listData, int total)
        {
            ListData = listData;
            Total = total;
        }

        public int Total { get; set; }
        public List<SearchOTCBaseModel> ListData { get; private set; }
    }
}
