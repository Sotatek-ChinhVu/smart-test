namespace Domain.Models.TenMst
{
    public interface ITenMstRepository
    {
        IEnumerable<TenMstModel> GetList(int hpId, int sinDate);
    }
}
