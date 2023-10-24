using Domain.Models.MstItem;
using Domain.Models.OrdInfDetails;

namespace EmrCloudApi.Responses.MstItem
{
    public class GetListYohoSetMstModelByUserIDResponse
    {
        public GetListYohoSetMstModelByUserIDResponse(List<YohoSetMstModel> data)
        {
            Data = data;
        }

        public List<YohoSetMstModel> Data { get; private set; }
    }
}
