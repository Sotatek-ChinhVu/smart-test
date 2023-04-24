using UseCase.Todo;

namespace EmrCloudApi.Requests.Todo;

public class UpsertTodoInfRequest
{
    public List<TodoInfDto> UpsertTodoInf { get; set; } = new List<TodoInfDto>();
}