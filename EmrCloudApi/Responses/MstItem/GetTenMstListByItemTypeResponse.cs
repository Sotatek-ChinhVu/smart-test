using Domain.Models.MstItem;

namespace EmrCloudApi.Responses.MstItem
{
    public class GetTenMstListByItemTypeResponse
    {
        public GetTenMstListByItemTypeResponse(List<TenMstMaintenanceModel> tenMsts)
        {
            TenMsts = tenMsts;
        }

        public List<TenMstMaintenanceModel> TenMsts { get; private set; }
    }
}
