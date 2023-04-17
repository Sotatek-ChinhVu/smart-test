using UseCase.Core.Sync.Core;

namespace UseCase.Todo.UpsertTodoInf;

public class UpsertTodoInfInputData : IInputData<UpsertTodoInfOutputData>
{
    public UpsertTodoInfInputData(List<InsertTodoInfDto> todoInfs, int userId, int hpId)
    {
        TodoInfs = todoInfs;
        UserId = userId;
        HpId = hpId;
    }

    public List<InsertTodoInfDto> TodoInfs { get; private set; }

    public int UserId { get; private set; }

    public int HpId { get; private set; }

    public List<InsertTodoInfDto> ToList()
    {
        return TodoInfs;
    }
}