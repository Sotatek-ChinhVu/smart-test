using Domain.Common;

namespace Domain.Models.Lock
{
    public interface ILockRepository : IRepositoryBase
    {
        Dictionary<int, Dictionary<int, string>> GetLockInf(int hpId);

        bool Unlock(int hpId, int userId, List<LockInfModel> lockInfModels, int managerKbn);

        List<LockInfModel> GetLockInfModels(int hpId, int userId, int managerKbn);

        bool ExistLock(int hpId, string functionCd, long ptId, int sinDate, long raiinNo);

        bool AddLock(int hpId, string functionCd, long ptId, int sinDate, long raiinNo, int userId, string tabKey, string loginKey);

        LockModel CheckOpenSpecialNote(int hpId, string functionCd, long ptId);

        List<LockModel> GetLock(int hpId, string functionCd, long ptId, int sinDate, long raiinNo, int userId);

        (List<long> raiinList, int removedCount) RemoveLock(int hpId, string functionCd, long ptId, int sinDate, long raiinNo, int userId, string tabKey);

        List<long> RemoveAllLock(int hpId, int userId);

        (List<long> raiinNoList, int removedCount) RemoveAllLock(int hpId, int userId, long ptId, int sinDate, string functionCd, string tabKey);

        List<long> RemoveAllLock(int hpId, int userId, string loginKey);

        bool ExtendTtl(int hpId, string functionCd, long ptId, int sinDate, long raiinNo, int userId);

        List<LockModel> GetLockInfo(int hpId, long ptId, List<string> listFunctionCD_B, int sinDate_B, long raiinNo);

        List<LockModel> GetVisitingLockStatus(int hpId, int userId, long ptId, string functionCode);

        string GetFunctionNameLock(string functionCode);

        List<ResponseLockModel> GetResponseLockModel(int hpId, long ptId, int sinDate, long raiinNo);

        List<ResponseLockModel> GetResponseLockModel(int hpId, List<long> raiinNoList);

        List<LockModel> CheckLockOpenAccounting(int hpId, long ptId, long raiinNo, int userId);

        LockModel CheckIsExistedOQLockInfo(int hpId, int userId, long ptId, string functionCd, long raiinNo, int sinDate);
    }
}
