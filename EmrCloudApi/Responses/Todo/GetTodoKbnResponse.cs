using Domain.Models.Todo;

namespace EmrCloudApi.Responses.Todo
{
    public class GetTodoKbnResponse
    {
        public GetTodoKbnResponse(List<TodoKbnMstModel> todoKbnMsts) 
        {
            TodoKbnMsts = todoKbnMsts;
        }

        public List<TodoKbnMstModel> TodoKbnMsts { get; private set; }
    }
}
