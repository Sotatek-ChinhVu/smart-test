using UseCase.Core.Sync.Core;

namespace UseCase.Todo.UpsertTodoInf;

public class UpsertTodoInfOutputData : IOutputData
{
    public UpsertTodoInfOutputData(UpsertTodoInfStatus status)
    {
        Status = status;
    }

    public UpsertTodoInfStatus Status { get; private set; }
}