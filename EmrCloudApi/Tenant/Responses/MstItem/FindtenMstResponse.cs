using Domain.Models.MstItem;

namespace EmrCloudApi.Tenant.Responses.MstItem
{
    public class FindtenMstResponse
    {
        public FindtenMstResponse(TenItemModel tenItemModel)
        {
            TenItemModel = tenItemModel;
        }
        public TenItemModel TenItemModel { get; private set; }
    }
}
