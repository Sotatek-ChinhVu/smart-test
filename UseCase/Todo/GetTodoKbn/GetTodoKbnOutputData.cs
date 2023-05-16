using Domain.Models.Todo;
using UseCase.Core.Sync.Core;

namespace UseCase.Todo.GetListTodoKbn;

public class GetTodoKbnOutputData : IOutputData
{
    public GetTodoKbnOutputData(GetTodoKbnStatus status, List<TodoKbnMstModel> todoKbnMstList) 
    { 
        Status = status;
        TodoKbnMstList = todoKbnMstList;
    }

    public GetTodoKbnStatus Status { get; private set; }

    public List<TodoKbnMstModel> TodoKbnMstList { get; private set; }
}
