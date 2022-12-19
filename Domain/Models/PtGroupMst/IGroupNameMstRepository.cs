namespace Domain.Models.PtGroupMst
{
    public interface IGroupNameMstRepository
    {
        bool SaveGroupNameMst(List<GroupNameMstModel> groupNameMsts, int hpId, int userId);
    }
}
