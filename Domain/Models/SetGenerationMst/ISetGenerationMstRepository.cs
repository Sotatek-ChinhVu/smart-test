using Domain.Common;

namespace Domain.Models.SetGenerationMst
{
    public interface ISetGenerationMstRepository : IRepositoryBase
    {
        IEnumerable<SetGenerationMstModel> GetList(int hpId, int sinDate);
        int GetGenerationId(int hpId, int sinDate);

        List<SetSendaiGenerationModel> GetListSendaiGeneration(int hpId);

        bool DeleteSetSenDaiGeneration(int generationId, int userId);

        bool AddSetSendaiGeneration(int userId, int hpId, int startDate);

        bool RestoreSetSendaiGeneration(int restoreGenerationId, int hpId, int userId);
    }
}
