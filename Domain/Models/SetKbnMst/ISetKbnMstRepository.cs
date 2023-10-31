using Domain.Common;

namespace Domain.Models.SetKbnMst
{
    public interface ISetKbnMstRepository : IRepositoryBase
    {
        IEnumerable<SetKbnMstModel> GetList(int hpId, int setKbnFrom, int setKbnTo);

        bool Upsert(int hpId, int userId, int generationId, List<SetKbnMstModel> setKbnMstModels);

        List<SetKbnMstModel> GetSetKbnMstListByGenerationId(int hpId, int generationId);
    }
}
