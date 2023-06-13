using UseCase.Core.Sync.Core;

namespace UseCase.Lock.Get
{
    public class CheckLockVisitingOutputData : IOutputData
    {
        public CheckLockVisitingOutputData(CheckLockVisitingStatus status)
        {
            Status = status;
        }

        public CheckLockVisitingStatus Status { get; private set; }
    }
}
