using Domain.Models.MstItem;

namespace EmrCloudApi.Tenant.Responses.MstItem
{
    public class SearchOTCResponse
    {
        public SearchOTCResponse(List<SearchOTCModel> listData)
        {
            ListData = listData;
        }

        public List<SearchOTCModel> ListData { get; private set; }
    }
}
