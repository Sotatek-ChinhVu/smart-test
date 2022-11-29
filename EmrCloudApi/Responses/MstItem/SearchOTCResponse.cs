using Domain.Models.MstItem;

namespace EmrCloudApi.Responses.MstItem
{
    public class SearchOTCResponse
    {
        public SearchOTCResponse(List<OtcItemModel> listData, int total)
        {
            ListData = listData;
            Total = total;
        }

        public int Total { get; set; }
        public List<OtcItemModel> ListData { get; private set; }
    }
}
