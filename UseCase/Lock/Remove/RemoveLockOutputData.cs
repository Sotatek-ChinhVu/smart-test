using UseCase.Core.Sync.Core;

namespace UseCase.Lock.Remove;

public class RemoveLockOutputData : IOutputData
{
    public RemoveLockStatus Status { get; private set; }

    public List<long> RaiinNoList { get; private set; }

    public RemoveLockOutputData(RemoveLockStatus status, List<long> raiinNoList)
    {
        Status = status;
        RaiinNoList = raiinNoList;
    }
}
