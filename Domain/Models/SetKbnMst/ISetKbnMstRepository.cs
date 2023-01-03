using Domain.Common;

namespace Domain.Models.SetKbnMst
{
    public interface ISetKbnMstRepository : IRepositoryBase
    {
        IEnumerable<SetKbnMstModel> GetList(int hpId, int setKbnFrom, int setKbnTo);
    }
}
