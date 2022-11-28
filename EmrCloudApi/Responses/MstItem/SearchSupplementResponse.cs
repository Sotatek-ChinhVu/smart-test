using Domain.Models.MstItem;

namespace EmrCloudApi.Responses.MstItem
{
    public class SearchSupplementResponse
    {
        public SearchSupplementResponse(List<SearchSupplementModel> listData, int total)
        {
            ListData = listData;
            Total = total;
        }

        public List<SearchSupplementModel> ListData { get; private set; }
        public int Total { get; set; }
    }
}
