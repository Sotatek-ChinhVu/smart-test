using UseCase.Core.Sync.Core;

namespace UseCase.Lock.Unlock
{
    public class UnlockOutputData : IOutputData
    {
        public UnlockOutputData(UnlockStatus status)
        {
            Status = status;
        }

        public UnlockStatus Status { get; private set; }
    }
}
