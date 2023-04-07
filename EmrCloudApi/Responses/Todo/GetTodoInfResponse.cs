using UseCase.Todo.GetTodoInf;

namespace EmrCloudApi.Responses.Todo
{
    public class GetTodoInfResponse
    {
        public GetTodoInfResponse(List<GetListTodoInfOutputItem> todoInfList)
        {
            TodoInfList = todoInfList;
        }

        public List<GetListTodoInfOutputItem> TodoInfList { get; private set; } = new List<GetListTodoInfOutputItem>();
    }
}
