using Domain.Common;
using Domain.Models.User;

namespace Domain.Models.UketukeSbtMst;

public interface IUketukeSbtMstRepository : IRepositoryBase
{
    UketukeSbtMstModel? GetByKbnId(int kbnId);

    List<UketukeSbtMstModel> GetList();

    void Upsert(List<UketukeSbtMstModel> upsertUketukeList, int userId, int hpId);
}
