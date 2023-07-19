using Domain.Models.Lock;
using UseCase.Core.Sync.Core;

namespace UseCase.Lock.CheckLockOpenAccounting;

public class CheckLockOpenAccountingOutputData : IOutputData
{
    public CheckLockOpenAccountingOutputData(CheckLockOpenAccountingStatus status, List<LockModel> lockInfs)
    {
        Status = status;
        LockInfs = lockInfs;
    }

    public CheckLockOpenAccountingStatus Status { get; private set; }

    public List<LockModel> LockInfs { get; private set; }
}
