using UseCase.Core.Sync.Core;

namespace UseCase.Lock.Remove
{
    public class RemoveLockOutputData : IOutputData
    {
        public RemoveLockStatus Status { get; private set; }

        public RemoveLockOutputData(RemoveLockStatus status)
        {
            Status = status;
        }
    }
}
