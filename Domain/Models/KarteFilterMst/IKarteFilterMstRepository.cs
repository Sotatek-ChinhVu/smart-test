namespace Domain.Models.KarteFilterMst;

public interface IKarteFilterMstRepository
{
    List<KarteFilterMstModel> GetList(int hpId, int userId);
    bool SaveList(List<KarteFilterMstModel> karteFilterMstModels, int userId, int hpId);
}
