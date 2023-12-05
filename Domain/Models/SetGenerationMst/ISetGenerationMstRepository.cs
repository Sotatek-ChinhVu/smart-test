using Domain.Common;
using Domain.Models.SetMst;

namespace Domain.Models.SetGenerationMst
{
    public interface ISetGenerationMstRepository : IRepositoryBase
    {
        IEnumerable<SetGenerationMstModel> GetList(int hpId, int sinDate);

        List<SetGenerationMstModel> GetSetGenerationMstList(int hpId);

        int GetGenerationId(int hpId, int sinDate);

        List<SetSendaiGenerationModel> GetListSendaiGeneration(int hpId);

        bool DeleteSetSenDaiGeneration(int hpId, int generationId, int userId, int startDate);

        AddSetSendaiModel? AddSetSendaiGeneration(int userId, int hpId, int startDate);

        GetCountProcessModel GetCountStepProcess(int targetGenerationId, int sourceGenerationId, int hpId, int userId);

        bool SaveCloneMstBackup(int targetGenerationId, int sourceGenerationId, int hpId, int userId);

        bool SaveCloneKbnMst(int targetGenerationId, int sourceGenerationId, int hpId, int userId);

        bool SaveCloneByomei(int hpId, int userId, Dictionary<int, SetMstModel> setMstDict, List<int> listMstDict);

        bool SaveCloneKarteInf(int hpId, int userId, Dictionary<int, SetMstModel> setMstDict, List<int> listMstDict);

        bool SaveCloneKarteImgInf(int hpId, Dictionary<int, SetMstModel> setMstDict, List<int> listMstDict);

        bool SaveCloneOdrInf(int hpId, int userId, Dictionary<int, SetMstModel> setMstDict, List<int> listMstDict);

        bool SaveCloneOdrInfDetail(int hpId, Dictionary<int, SetMstModel> setMstDict, List<int> listMstDict);

        bool SaveCloneOdrInfCmt(int hpId, Dictionary<int, SetMstModel> setMstDict, List<int> listMstDict);

        AddSetSendaiModel? RestoreSetSendaiGeneration(int restoreGenerationId, int hpId, int userId);

        void RestoreByomeiSetGeneration(int hpId, int userId, int targetGeneration, int sourceGenerationId);

        void RestoreListSetGeneration(int hpId, int userId, int targetGeneration, int sourceGenerationId);

        List<ListSetGenerationMstModel> GetAll(int hpId);

        AddSetSendaiModel? AddByomeiSetGenerationMst(int hpId, int userId, int startDate);

        AddSetSendaiModel? AddListSetGenerationMst(int hpId, int userId, int startDate);

        void CloneByomeiSetGeneration(int hpId, int userId, int targetGeneration, int sourceGenerationId);

        void CloneListSetGeneration(int hpId, int userId, int targetGeneration, int sourceGenerationId);

        IEnumerable<SetGenerationMstModel> ReloadCache(int hpId, bool flag = false);
    }
}
