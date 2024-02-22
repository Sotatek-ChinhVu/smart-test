using Domain.Common;
using Domain.Models.LockInf;

namespace Domain.Models.ReceptionLock
{
    public interface IReceptionLockRepository : IRepositoryBase
    {
        public List<ReceptionLockModel> ReceptionLockModel(int sinDate, long ptId, long raiinNo, string functionCd);
    }
}
