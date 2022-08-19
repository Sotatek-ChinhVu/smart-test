using Domain.Models.InputItem;

namespace EmrCloudApi.Tenant.Responses.InputItem
{
    public class SearchInputItemResponse
    {
        public SearchInputItemResponse(List<InputItemModel> listData, int totalCount)
        {
            ListData = listData;
            TotalCount = totalCount;
        }

        public List<InputItemModel> ListData { get; private set; }

        public int TotalCount { get; private set; }
    }
}
