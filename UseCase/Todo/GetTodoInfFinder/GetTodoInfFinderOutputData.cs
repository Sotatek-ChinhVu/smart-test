using UseCase.Core.Sync.Core;

namespace UseCase.Todo.GetTodoInfFinder;

public class GetTodoInfFinderOutputData : IOutputData
{
    public GetTodoInfFinderOutputData(GetTodoInfFinderStatus status, List<GetListTodoInfFinderOutputItem> todoInfList)
    {
        Status = status;
        TodoInfList = todoInfList;
    }

    public GetTodoInfFinderStatus Status { get; private set; }

    public List<GetListTodoInfFinderOutputItem> TodoInfList { get; private set; }
}
