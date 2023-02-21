using Domain.Common;

namespace Domain.Models.PtGroupMst
{
    public interface IGroupNameMstRepository : IRepositoryBase
    {
        bool SaveGroupNameMst(List<GroupNameMstModel> groupNameMsts, int hpId, int userId);

        List<GroupNameMstModel> GetListGroupNameMst(int hpId);

        bool IsInUseGroupName(int groupId, string groupCode);

        bool IsInUseGroupItem(int groupId, string groupCode);
    }
}
