using Domain.Models.MstItem;

namespace EmrCloudApi.Responses.MstItem
{
    public class GetTenMstByCodeResponse
    {
        public GetTenMstByCodeResponse(TenItemModel? data)
        {
            Data = data;
        }

        public TenItemModel? Data { get; private set; }
    }
}
