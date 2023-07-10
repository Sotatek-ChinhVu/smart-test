using UseCase.Core.Sync.Core;

namespace UseCase.Lock.CheckLockOpenAccounting;

public interface ICheckLockOpenAccountingInputPort : IInputPort<CheckLockOpenAccountingInputData, CheckLockOpenAccountingOutputData>
{
}
