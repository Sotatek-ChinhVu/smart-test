using Domain.Models.Todo;
using UseCase.Core.Sync.Core;

namespace UseCase.Todo.TodoGrpMst;

public class UpsertTodoGrpMstInputData : IInputData<UpsertTodoGrpMstOutputData>
{
    public UpsertTodoGrpMstInputData(List<InsertTodoGrpMstDto> todoGrpMsts, int userId, int hpId)
    {
        TodoGrpMsts = todoGrpMsts;
        UserId = userId;
        HpId = hpId;
    }

    public List<InsertTodoGrpMstDto> TodoGrpMsts { get; private set; }

    public int UserId { get; private set; }

    public int HpId { get; private set; }

    public List<InsertTodoGrpMstDto> ToList()
    {
        return TodoGrpMsts;
    }
}
