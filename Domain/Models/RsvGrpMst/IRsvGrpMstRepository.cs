namespace Domain.Models.RsvGrpMst
{
    public interface IRsvGrpMstRepository
    {
        List<RsvGrpMstModel> GetList(int hpId);
    }
}
