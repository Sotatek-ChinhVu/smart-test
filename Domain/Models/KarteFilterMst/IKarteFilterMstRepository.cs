namespace Domain.Models.KarteFilterMst;

public interface IKarteFilterMstRepository
{
    List<KarteFilterMst> GetList(int hpId, int userId);
}
