using Domain.Models.Todo;
using UseCase.Core.Sync.Core;

namespace UseCase.Todo.TodoGrpMst;

public class UpsertTodoGrpMstInputData : IInputData<UpsertTodoGrpMstOutputData>
{
    public UpsertTodoGrpMstInputData(List<TodoGrpMstModel> todoGrpMsts, int userId, int hpId)
    {
        TodoGrpMsts = todoGrpMsts;
        UserId = userId;
        HpId = hpId;
    }

    public List<TodoGrpMstModel> TodoGrpMsts { get; private set; }

    public int UserId { get; private set; }

    public int HpId { get; private set; }

    public List<TodoGrpMstModel> ToList()
    {
        return TodoGrpMsts;
    }
}
