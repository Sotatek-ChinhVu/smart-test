using Domain.Common;

namespace Domain.Models.UketukeSbtMst;

public interface IUketukeSbtMstRepository : IRepositoryBase
{
    UketukeSbtMstModel? GetByKbnId(int kbnId);
    List<UketukeSbtMstModel> GetList();
}
