using Domain.Common;

namespace Domain.Models.Lock
{
    public interface ILockRepository : IRepositoryBase
    {
        bool ExistLock(int hpId, string functionCd, long ptId, int sinDate, long raiinNo);

        bool AddLock(int hpId, string functionCd, long ptId, int sinDate, long raiinNo, int userId);

        List<LockModel> GetLock(int hpId, string functionCd, long ptId, int sinDate, long raiinNo, int userId);

        bool RemoveLock(int hpId, string functionCd, long ptId, int sinDate, long raiinNo, int userId);

        bool RemoveAllLock(int hpId, int userId);
    }
}
