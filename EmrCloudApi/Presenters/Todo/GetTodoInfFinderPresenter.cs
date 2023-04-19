using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Todo;
using UseCase.Todo.GetTodoInfFinder;

namespace EmrCloudApi.Presenters.Todo;

public class GetTodoInfFinderPresenter : IGetTodoInfFinderOutputPort
{
    public Response<GetTodoInfFinderResponse> Result { get; private set; } = new Response<GetTodoInfFinderResponse>();
    public void Complete(GetTodoInfFinderOutputData outputData)
    {
        Result.Data = new GetTodoInfFinderResponse(outputData.TodoInfList);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }
    private static string GetMessage(GetTodoInfFinderStatus status) => status switch
    {
        GetTodoInfFinderStatus.Success => ResponseMessage.Success,
        GetTodoInfFinderStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        GetTodoInfFinderStatus.InvalidTodoNo => ResponseMessage.InvalidTodoNo,
        GetTodoInfFinderStatus.InvalidTodoEdaNo => ResponseMessage.InvalidTodoEdaNo,
        _ => string.Empty
    };
}
