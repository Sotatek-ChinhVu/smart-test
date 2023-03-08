using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.Recaculate
{
    public interface IRecaculationInputPort : IInputPort<RecaculationInputData, RecaculationOutputData>
    {
    }
}
