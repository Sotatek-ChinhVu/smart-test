using UseCase.Core.Sync.Core;
using UseCase.Todo.TodoGrpMst;

namespace UseCase.Todo.UpsertTodoGrpMst
{
    public interface IUpsertTodoGrpMstInputPort : IInputPort<UpsertTodoGrpMstInputData, UpsertTodoGrpMstOutputData>
    {
    }
}
