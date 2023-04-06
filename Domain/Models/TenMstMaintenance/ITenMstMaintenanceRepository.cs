using Domain.Common;

namespace Domain.Models.TenMstMaintenance
{
    public interface ITenMstMaintenanceRepository : IRepositoryBase
    {
        List<TenMstOriginModel> GetGroupTenMst(string itemCd);
    }
}
