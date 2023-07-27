using UseCase.Core.Sync.Core;

namespace UseCase.ReceiptCheck.Recalculation
{
    public interface IReceiptCheckRecalculationInputPort : IInputPort<ReceiptCheckRecalculationInputData, ReceiptCheckRecalculationOutputData>
    {
    }
}