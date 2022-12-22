using Domain.Common;

namespace Domain.Models.SetGenerationMst
{
    public interface ISetGenerationMstRepository : IRepositoryBase
    {
        IEnumerable<SetGenerationMstModel> GetList(int hpId, int sinDate);
        int GetGenerationId(int hpId, int sinDate);
    }
}
