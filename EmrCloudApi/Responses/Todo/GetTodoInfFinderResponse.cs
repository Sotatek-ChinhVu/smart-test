using UseCase.Todo.GetTodoInfFinder;

namespace EmrCloudApi.Responses.Todo
{
    public class GetTodoInfFinderResponse
    {
        public GetTodoInfFinderResponse(List<GetListTodoInfFinderOutputItem> todoInfList)
        {
            TodoInfList = todoInfList;
        }

        public List<GetListTodoInfFinderOutputItem> TodoInfList { get; private set; }
    }
}
