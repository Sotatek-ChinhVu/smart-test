using Domain.Models.Todo;
using UseCase.Core.Sync.Core;

namespace UseCase.Todo.TodoGrpMst;

public class UpsertTodoGrpMstOutputData : IOutputData
{
    public UpsertTodoGrpMstOutputData(UpsertTodoGrpMstStatus status)
    {
        Status = status;
    }

    public UpsertTodoGrpMstStatus Status { get; private set; }
}
