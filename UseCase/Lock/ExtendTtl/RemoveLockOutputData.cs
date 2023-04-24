using UseCase.Core.Sync.Core;

namespace UseCase.Lock.ExtendTtl
{
    public class ExtendTtlLockOutputData : IOutputData
    {
        public ExtendTtlLockStatus Status { get; private set; }

        public ExtendTtlLockOutputData(ExtendTtlLockStatus status)
        {
            Status = status;
        }
    }
}
