using UseCase.Core.Sync.Core;

namespace UseCase.NextOrder.Upsert
{
    public interface IUpsertNextOrderInputPort : IInputPort<UpsertNextOrderInputData, UpsertNextOrderOutputData>
    {
    }
}
