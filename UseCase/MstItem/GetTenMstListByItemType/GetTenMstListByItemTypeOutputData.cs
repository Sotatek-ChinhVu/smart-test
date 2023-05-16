using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetTenMstListByItemType
{
    public class GetTenMstListByItemTypeOutputData : IOutputData
    {
        public GetTenMstListByItemTypeOutputData(GetTenMstListByItemTypeStatus status, List<TenMstMaintenanceModel> tenMsts)
        {
            Status = status;
            TenMsts = tenMsts;
        }

        public GetTenMstListByItemTypeStatus Status { get; private set; }

        public List<TenMstMaintenanceModel> TenMsts { get; private set; }
    }
}
