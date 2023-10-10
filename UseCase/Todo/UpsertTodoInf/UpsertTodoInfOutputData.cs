using UseCase.Core.Sync.Core;
using UseCase.Todo.GetTodoInfFinder;

namespace UseCase.Todo.UpsertTodoInf;

public class UpsertTodoInfOutputData : IOutputData
{
    public UpsertTodoInfOutputData(List<GetListTodoInfFinderOutputItem> outputItems, UpsertTodoInfStatus status)
    {
        OutputItems = outputItems;
        Status = status;
    }

    public List<GetListTodoInfFinderOutputItem> OutputItems { get; private set; }

    public UpsertTodoInfStatus Status { get; private set; }
}