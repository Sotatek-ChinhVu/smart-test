using Domain.Common;

namespace Domain.Models.Lock
{
    public interface ILockRepository : IRepositoryBase
    {
        bool ExistLock(int hpId, string functionCd, long ptId, int sinDate, long raiinNo);

        bool AddLock(int hpId, string functionCd, long ptId, int sinDate, long raiinNo, int userId, string token);

        LockModel CheckOpenSpecialNote(int hpId, string functionCd, long ptId);

        List<LockModel> GetLock(int hpId, string functionCd, long ptId, int sinDate, long raiinNo, int userId);

        bool RemoveLock(int hpId, string functionCd, long ptId, int sinDate, long raiinNo, int userId);

        bool RemoveAllLock(int hpId, int userId);

        bool ExtendTtl(int hpId, string functionCd, long ptId, int sinDate, long raiinNo, int userId);

        List<LockModel> GetLockInfo(int hpId, long ptId, List<string> lisFunctionCd_B, int sinDate_B, long raiinNo);

        bool GetVisitingLockStatus(int hpId, int userId, long ptId, string functionCode);

        string GetFunctionNameLock(string functionCode);
    }
}
