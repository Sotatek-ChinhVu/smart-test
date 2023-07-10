using Domain.Models.Lock;
using UseCase.Core.Sync.Core;

namespace UseCase.Lock.CheckExistFunctionCode;

public class CheckExistFunctionCodeOutputData : IOutputData
{
    public CheckExistFunctionCodeOutputData(LockModel lockInf)
    {
        Status = (lockInf.UserId == 0 && string.IsNullOrEmpty(lockInf.FunctionCode)) ? CheckExistFunctionCodeStatus.NoData : CheckExistFunctionCodeStatus.Successed;
        LockInf = lockInf;
    }

    public CheckExistFunctionCodeStatus Status { get; private set; }

    public LockModel LockInf { get; private set; }
}
