using Domain.Models.Lock;
using UseCase.Core.Sync.Core;

namespace UseCase.Lock.Add
{
    public class AddLockOutputData : IOutputData
    {
        public LockModel LockInf { get; set; }

        public AddLockStatus Status { get; private set; }

        public AddLockOutputData(AddLockStatus status, LockModel lockInf)
        {
            Status = status;
            LockInf = lockInf;
        }
    }
}
