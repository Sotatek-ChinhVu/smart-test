using UseCase.Core.Sync.Core;

namespace UseCase.Todo.UpsertTodoInf;

public class UpsertTodoInfInputData : IInputData<UpsertTodoInfOutputData>
{
    public UpsertTodoInfInputData(List<TodoInfDto> todoInfs, int userId, int hpId)
    {
        TodoInfs = todoInfs;
        UserId = userId;
        HpId = hpId;
    }

    public List<TodoInfDto> TodoInfs { get; private set; }

    public int UserId { get; private set; }

    public int HpId { get; private set; }

    public List<TodoInfDto> ToList()
    {
        return TodoInfs;
    }
}