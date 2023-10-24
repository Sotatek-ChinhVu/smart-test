using Domain.Models.MstItem;

namespace EmrCloudApi.Responses.MstItem
{
    public class CompareTenMstResponse
    {
        public CompareTenMstResponse(List<CompareTenMstModel> listData)
        {
            ListData = listData;
        }

        public List<CompareTenMstModel> ListData { get; private set; }
    }
}
