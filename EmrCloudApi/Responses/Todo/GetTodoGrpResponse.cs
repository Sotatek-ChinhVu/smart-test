using UseCase.Todo;

namespace EmrCloudApi.Responses.Todo
{
    public class GetTodoGrpResponse
    {
        public GetTodoGrpResponse(List<TodoGrpMstDto> todoGrpMstDtos)
        {
            TodoGrpMstDtos = todoGrpMstDtos;
        }

        public List<TodoGrpMstDto> TodoGrpMstDtos { get; private set; }
    }
}
