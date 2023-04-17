using UseCase.Core.Sync.Core;

namespace UseCase.Todo.UpsertTodoGrpMst;

public class UpsertTodoGrpMstInputData : IInputData<UpsertTodoGrpMstOutputData>
{
    public UpsertTodoGrpMstInputData(List<TodoGrpMstDto> todoGrpMsts, int userId, int hpId)
    {
        TodoGrpMsts = todoGrpMsts;
        UserId = userId;
        HpId = hpId;
    }

    public List<TodoGrpMstDto> TodoGrpMsts { get; private set; }

    public int UserId { get; private set; }

    public int HpId { get; private set; }

    public List<TodoGrpMstDto> ToList()
    {
        return TodoGrpMsts;
    }
}
