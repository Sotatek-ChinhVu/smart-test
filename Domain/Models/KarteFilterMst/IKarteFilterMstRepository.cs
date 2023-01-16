using Domain.Common;

namespace Domain.Models.KarteFilterMst;

public interface IKarteFilterMstRepository : IRepositoryBase
{
    List<KarteFilterMstModel> GetList(int hpId, int userId);
    KarteFilterMstModel Get(int hpId, int userId, long filterId);
    bool SaveList(List<KarteFilterMstModel> karteFilterMstModels, int userId, int hpId);
}
