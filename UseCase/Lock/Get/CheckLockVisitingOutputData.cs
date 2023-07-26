using Domain.Models.Lock;
using UseCase.Core.Sync.Core;

namespace UseCase.Lock.Get;

public class CheckLockVisitingOutputData : IOutputData
{
    public CheckLockVisitingOutputData(CheckLockVisitingStatus status, List<LockModel> lockInfs)
    {
        Status = status;
        LockInfs = lockInfs;
    }

    public CheckLockVisitingStatus Status { get; private set; }

    public List<LockModel> LockInfs { get; private set; }
}
