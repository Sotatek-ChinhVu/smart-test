using Domain.Models.Lock;
using UseCase.Core.Sync.Core;

namespace UseCase.Lock.Get
{
    public class GetLockInfoOutputData : IOutputData
    {
        public GetLockInfoOutputData(List<LockModel> lockInfs, GetLockInfoStatus status)
        {
            LockInfs = lockInfs;
            Status = status;
        }

        public List<LockModel> LockInfs { get; private set; }
        public GetLockInfoStatus Status { get; private set; }
    }
}
