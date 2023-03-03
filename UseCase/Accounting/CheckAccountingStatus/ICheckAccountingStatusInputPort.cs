using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.CheckAccountingStatus
{
    public interface ICheckAccountingStatusInputPort : IInputPort<CheckAccountingStatusInputData, CheckAccountingStatusOutputData>
    {
    }
}
