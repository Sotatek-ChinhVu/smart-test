using Domain.Common;

namespace Domain.Models.SetKbnMst
{
    public interface ISetKbnMstRepository : IRepositoryBase
    {
        IEnumerable<SetKbnMstModel> GetList(int hpId, int setKbnFrom, int setKbnTo);

        List<SetKbnMstModel> GetListKbnMst(int hpId, List<int> grpCodes, int sinDate);
    }
}
