using Domain.Common;

namespace Domain.Models.KarteKbnMst
{
    public interface IKarteKbnMstRepository : IRepositoryBase
    {
        List<KarteKbnMstModel> GetList(int hpId, bool isDeleted);
    }
}
