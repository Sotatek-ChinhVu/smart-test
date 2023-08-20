using EmrCloudApi.Constants;
using EmrCloudApi.Responses.Online;
using EmrCloudApi.Responses;
using UseCase.Online.GetRegisterdPatientsFromOnline;
using EmrCloudApi.Responses.Online.Dto;

namespace EmrCloudApi.Presenters.Online;

public class GetRegisterdPatientsFromOnlinePresenter : IGetRegisterdPatientsFromOnlineOutputPort
{
    public Response<GetRegisterdPatientsFromOnlineResponse> Result { get; private set; } = new();

    public void Complete(GetRegisterdPatientsFromOnlineOutputData output)
    {
        Result.Data = new GetRegisterdPatientsFromOnlineResponse(output.OnlineConfirmationHistoryList.Select(item => new OnlineConfirmationHistoryDto(item)).ToList());
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetRegisterdPatientsFromOnlineStatus status) => status switch
    {
        GetRegisterdPatientsFromOnlineStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}

