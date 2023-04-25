using UseCase.Core.Sync.Core;

namespace UseCase.Lock.ExtendTtl
{
    public interface IExtendTtlLockInputPort : IInputPort<ExtendTtlLockInputData, ExtendTtlLockOutputData>
    {
    }
}
