using Domain.Models.Lock;
using UseCase.Core.Sync.Core;

namespace UseCase.Lock.Check
{
    public class CheckLockOutputData : IOutputData
    {
        public LockModel LockInf { get; set; }

        public CheckLockStatus Status { get; private set; }

        public CheckLockOutputData(CheckLockStatus status, LockModel lockInf)
        {
            Status = status;
            LockInf = lockInf;
        }
    }
}
