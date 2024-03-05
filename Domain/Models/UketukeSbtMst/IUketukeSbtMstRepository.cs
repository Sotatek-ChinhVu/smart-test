using Domain.Common;
using Domain.Models.User;

namespace Domain.Models.UketukeSbtMst;

public interface IUketukeSbtMstRepository : IRepositoryBase
{
    UketukeSbtMstModel GetByKbnId(int hpId, int kbnId);

    List<UketukeSbtMstModel> GetList(int hpId);

    void Upsert(List<UketukeSbtMstModel> upsertUketukeList, int userId, int hpId);
}
