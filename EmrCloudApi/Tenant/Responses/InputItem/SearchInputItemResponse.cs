using Domain.Models.InputItem;

namespace EmrCloudApi.Tenant.Responses.InputItem
{
    public class SearchInputItemResponse
    {
        public SearchInputItemResponse(List<InputItemModel> listData)
        {
            ListData = listData;
        }

        public List<InputItemModel> ListData { get; private set; }
    }
}
