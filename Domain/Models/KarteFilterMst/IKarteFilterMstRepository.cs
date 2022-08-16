namespace Domain.Models.KarteFilterMst;

public interface IKarteFilterMstRepository
{
    List<KarteFilterMstModel> GetList(int hpId, int userId);
}
