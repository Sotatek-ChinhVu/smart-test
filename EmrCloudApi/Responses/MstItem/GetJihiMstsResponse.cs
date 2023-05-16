using Domain.Models.MstItem;

namespace EmrCloudApi.Responses.MstItem
{
    public class GetJihiMstsResponse
    {
        public GetJihiMstsResponse(List<JihiSbtMstModel> listData)
        {
            ListData = listData;
        }

        public List<JihiSbtMstModel> ListData { get; private set; }
    }
}
