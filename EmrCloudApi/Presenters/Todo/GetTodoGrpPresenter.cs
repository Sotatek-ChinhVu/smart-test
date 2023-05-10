using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Todo;
using UseCase.Todo.GetTodoGrp;

namespace EmrCloudApi.Presenters.Todo;

public class GetTodoGrpPresenter : IGetTodoGrpOutputPort
{
    public Response<GetTodoGrpResponse> Result { get; private set; } = new Response<GetTodoGrpResponse>();
    public void Complete(GetTodoGrpOutputData outputData)
    {
        Result.Data = new GetTodoGrpResponse(outputData.TodoGrpMstDtos);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }
    private static string GetMessage(GetTodoGrpStatus status) => status switch
    {
        GetTodoGrpStatus.Success => ResponseMessage.Success,
        GetTodoGrpStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        _ => string.Empty
    };
}
