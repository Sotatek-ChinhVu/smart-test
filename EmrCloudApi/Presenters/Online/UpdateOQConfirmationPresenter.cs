using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Online;
using UseCase.Online.UpdateOQConfirmation;

namespace EmrCloudApi.Presenters.Online;

public class UpdateOQConfirmationPresenter : IUpdateOQConfirmationOutputPort
{
    public Response<UpdateOQConfirmationResponse> Result { get; private set; } = new();

    public void Complete(UpdateOQConfirmationOutputData output)
    {
        Result.Data = new UpdateOQConfirmationResponse(output.Status == UpdateOQConfirmationStatus.Successed);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(UpdateOQConfirmationStatus status) => status switch
    {
        UpdateOQConfirmationStatus.Successed => ResponseMessage.Success,
        UpdateOQConfirmationStatus.Failed => ResponseMessage.Failed,
        UpdateOQConfirmationStatus.InvalidId => ResponseMessage.InvalidOnlineId,
        _ => string.Empty
    };
}

