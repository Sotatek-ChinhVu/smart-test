using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Todo;
using UseCase.Todo.GetTodoInf;

namespace EmrCloudApi.Presenters.Todo;

public class GetTodoInfPresenter : IGetTodoInfOutputPort
{
    public Response<GetTodoInfResponse> Result { get; private set; } = new Response<GetTodoInfResponse>();
    public void Complete(GetTodoInfOutputData outputData)
    {
        Result.Data = new GetTodoInfResponse(outputData.TodoInfList);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }
    private static string GetMessage(GetTodoInfStatus status) => status switch
    {
        GetTodoInfStatus.Success => ResponseMessage.Success,
        GetTodoInfStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        GetTodoInfStatus.InvalidTodoNo => ResponseMessage.InvalidTodoNo,
        GetTodoInfStatus.InvalidTodoEdaNo => ResponseMessage.InvalidTodoEdaNo,
        _ => string.Empty
    };
}
