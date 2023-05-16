using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Todo;
using UseCase.Todo.GetListTodoKbn;

namespace EmrCloudApi.Presenters.Todo;

public class GetTodoKbnPresenter : IGetTodoKbnOutputPort
{
    public Response<GetTodoKbnResponse> Result { get; private set; } = new Response<GetTodoKbnResponse>();
    public void Complete(GetTodoKbnOutputData outputData)
    {
        Result.Data = new GetTodoKbnResponse(outputData.TodoKbnMstList);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private static string GetMessage(GetTodoKbnStatus status) => status switch
    {
        GetTodoKbnStatus.Success => ResponseMessage.Success,
        _ => string.Empty
    };
}
