using UseCase.Core.Sync.Core;

namespace UseCase.Lock.CheckLockOpenAccounting;

public class CheckLockOpenAccountingOutputData : IOutputData
{
    public CheckLockOpenAccountingOutputData(CheckLockOpenAccountingStatus status)
    {
        Status = status;
    }

    public CheckLockOpenAccountingStatus Status { get; private set; }
}
