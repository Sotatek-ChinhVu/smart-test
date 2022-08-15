namespace Domain.Models.TenMst
{
    public interface ITenMstRepository
    {
        List<TenMstModel> GetList(int hpId, int sinDate);
    }
}
