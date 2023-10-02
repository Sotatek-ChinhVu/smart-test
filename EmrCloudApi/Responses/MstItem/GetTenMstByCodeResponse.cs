using Domain.Models.MstItem;

namespace EmrCloudApi.Responses.MstItem
{
    public class GetTenMstByCodeResponse
    {
        public GetTenMstByCodeResponse(TenMstModel data)
        {
            Data = data;
        }

        public TenMstModel Data { get; private set; }
    }
}
