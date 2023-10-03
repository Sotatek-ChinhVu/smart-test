using Domain.Common;
using Domain.Models.Document;

namespace Domain.Models.Lock
{
    public interface ILockRepository : IRepositoryBase
    {
        bool Unlock(int hpId, int userId, List<LockModel> lockInfModels);

        bool ExistLock(int hpId, string functionCd, long ptId, int sinDate, long raiinNo);

        bool AddLock(int hpId, string functionCd, long ptId, int sinDate, long raiinNo, int userId, string tabKey, string loginKey);

        LockModel CheckOpenSpecialNote(int hpId, string functionCd, long ptId);

        List<LockModel> GetLock(int hpId, string functionCd, long ptId, int sinDate, long raiinNo, int userId);

        List<long> RemoveLock(int hpId, string functionCd, long ptId, int sinDate, long raiinNo, int userId, string tabKey);

        List<long> RemoveAllLock(int hpId, int userId);

        List<long> RemoveAllLock(int hpId, int userId, long ptId, int sinDate, string functionCd, string tabKey);

        List<long> RemoveAllLock(int hpId, int userId, string loginKey);

        bool ExtendTtl(int hpId, string functionCd, long ptId, int sinDate, long raiinNo, int userId);

        List<LockModel> GetLockInfo(int hpId, long ptId, List<string> lisFunctionCd_B, int sinDate_B, long raiinNo);

        List<LockModel> GetVisitingLockStatus(int hpId, int userId, long ptId, string functionCode);

        string GetFunctionNameLock(string functionCode);

        List<ResponseLockModel> GetResponseLockModel(int hpId, long ptId, int sinDate, long raiinNo);

        List<ResponseLockModel> GetResponseLockModel(int hpId, List<long> raiinNoList);

        List<LockModel> CheckLockOpenAccounting(int hpId, long ptId, long raiinNo, int userId);
    }
}
