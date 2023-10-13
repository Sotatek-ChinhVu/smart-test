using Domain.Models.Lock;
using UseCase.Core.Sync.Core;

namespace UseCase.Lock.GetLockInf
{
    public class GetLockInfOutputData : IOutputData
    {
        public GetLockInfOutputData(List<LockInfModel> lockInfs, Dictionary<int, Dictionary<int, string>> userLocks, GetLockInfStatus status)
        {
            LockInfs = lockInfs;
            UserLocks = userLocks;
            Status = status;
        }

        public List<LockInfModel> LockInfs { get; private set; }

        public Dictionary<int, Dictionary<int, string>> UserLocks { get; private set; }

        public GetLockInfStatus Status { get; private set; }
    }
}
