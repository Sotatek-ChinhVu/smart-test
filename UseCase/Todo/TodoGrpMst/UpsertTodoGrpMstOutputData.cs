using Domain.Models.Todo;
using UseCase.Core.Sync.Core;

namespace UseCase.Todo.TodoGrpMst;

public class UpsertTodoGrpMstOutputData : IOutputData
{
    public UpsertTodoGrpMstOutputData(TodoGrpMstConstant status)
    {
        Status = status;
    }

    public UpsertTodoGrpMstOutputData(TodoGrpMstConstant status, List<TodoGrpMstModel> todoGrpMstList)
    {
        Status = status;
        TodoGrpMstList = todoGrpMstList;
    }

    public TodoGrpMstConstant Status { get; private set; }

    public List<TodoGrpMstModel> TodoGrpMstList { get; private set; } = new List<TodoGrpMstModel>();
}
