using UseCase.Core.Sync.Core;

namespace UseCase.ReceiptCheck
{
    public interface IRecalculationInputPort : IInputPort<ReceiptCheckRecalculationInputData, ReceiptCheckRecalculationOutputData>
    {
    }
}