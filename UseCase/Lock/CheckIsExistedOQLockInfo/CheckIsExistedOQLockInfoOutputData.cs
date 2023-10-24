using Domain.Models.Lock;
using UseCase.Core.Sync.Core;

namespace UseCase.Lock.CheckIsExistedOQLockInfo;

public class CheckIsExistedOQLockInfoOutputData : IOutputData
{
    public CheckIsExistedOQLockInfoOutputData(LockModel lockModel, CheckIsExistedOQLockInfoStatus status)
    {
        LockModel = lockModel;
        Status = status;
    }

    public LockModel LockModel { get; private set; }

    public CheckIsExistedOQLockInfoStatus Status { get; private set; }
}
