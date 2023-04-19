using UseCase.Core.Sync.Core;

namespace UseCase.Todo.UpsertTodoGrpMst;

public class UpsertTodoGrpMstOutputData : IOutputData
{
    public UpsertTodoGrpMstOutputData(UpsertTodoGrpMstStatus status)
    {
        Status = status;
    }

    public UpsertTodoGrpMstStatus Status { get; private set; }
}
