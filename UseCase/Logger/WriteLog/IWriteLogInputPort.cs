using UseCase.Core.Sync.Core;

namespace UseCase.Logger
{
    public interface IWriteLogInputPort : IInputPort<WriteLogInputData, WriteLogOutputData>
    {
    }
}
