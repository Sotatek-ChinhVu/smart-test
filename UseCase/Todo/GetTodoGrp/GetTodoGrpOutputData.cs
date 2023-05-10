using UseCase.Core.Sync.Core;

namespace UseCase.Todo.GetTodoGrp;

public class GetTodoGrpOutputData : IOutputData
{
    public GetTodoGrpOutputData(GetTodoGrpStatus status, List<TodoGrpMstDto> todoGrpMstDtos)
    {
        Status = status;
        TodoGrpMstDtos = todoGrpMstDtos;
    }

    public GetTodoGrpStatus Status { get; private set; }

    public List<TodoGrpMstDto> TodoGrpMstDtos { get; private set; }
}
