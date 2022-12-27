using Domain.Common;

namespace Domain.Models.PtGroupMst
{
    public interface IGroupNameMstRepository : IRepositoryBase
    {
        bool SaveGroupNameMst(List<GroupNameMstModel> groupNameMsts, int hpId, int userId);
    }
}
