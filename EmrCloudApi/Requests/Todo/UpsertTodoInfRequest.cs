using UseCase.Todo;

namespace EmrCloudApi.Requests.Todo;

public class UpsertTodoInfRequest
{
    public List<InsertTodoInfDto> UpsertTodoInf { get; set; } = new List<InsertTodoInfDto>();
}