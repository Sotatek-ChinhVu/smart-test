using UseCase.Core.Sync.Core;

namespace UseCase.Lock.Check
{
    public interface ICheckLockInputPort : IInputPort<CheckLockInputData, CheckLockOutputData>
    {
    }
}
