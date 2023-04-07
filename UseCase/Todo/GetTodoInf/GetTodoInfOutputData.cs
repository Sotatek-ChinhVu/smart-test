using UseCase.Core.Sync.Core;

namespace UseCase.Todo.GetTodoInf;

public class GetTodoInfOutputData : IOutputData
{
    public GetTodoInfOutputData(GetTodoInfStatus status, List<GetListTodoInfOutputItem> todoInfList)
    {
        Status = status;
        TodoInfList = todoInfList;
    }

    public GetTodoInfStatus Status { get; private set; }

    public List<GetListTodoInfOutputItem> TodoInfList { get; private set; }
}
