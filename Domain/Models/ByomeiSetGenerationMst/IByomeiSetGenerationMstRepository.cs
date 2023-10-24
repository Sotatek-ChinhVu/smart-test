using Domain.Common;

namespace Domain.Models.ByomeiSetGenerationMst
{
    public interface IByomeiSetGenerationMstRepository : IRepositoryBase
    {
        List<ByomeiSetGenerationMstModel> GetAll(int hpId);
    }
}
