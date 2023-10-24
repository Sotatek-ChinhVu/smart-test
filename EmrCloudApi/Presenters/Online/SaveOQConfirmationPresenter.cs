using EmrCloudApi.Constants;
using EmrCloudApi.Responses.Online;
using EmrCloudApi.Responses;
using UseCase.Online.SaveOQConfirmation;

namespace EmrCloudApi.Presenters.Online;

public class SaveOQConfirmationPresenter : ISaveOQConfirmationOutputPort
{
    public Response<SaveOQConfirmationResponse> Result { get; private set; } = new();

    public void Complete(SaveOQConfirmationOutputData output)
    {
        Result.Data = new SaveOQConfirmationResponse(output.Status == SaveOQConfirmationStatus.Successed);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(SaveOQConfirmationStatus status) => status switch
    {
        SaveOQConfirmationStatus.Successed => ResponseMessage.Success,
        SaveOQConfirmationStatus.Failed => ResponseMessage.Failed,
        SaveOQConfirmationStatus.InvalidPtId => ResponseMessage.InvalidPtId,
        SaveOQConfirmationStatus.InvalidId => ResponseMessage.InvalidOnlineId,
        SaveOQConfirmationStatus.InvalidConfirmationResult => ResponseMessage.InvalidConfirmationResult,
        _ => string.Empty
    };
}
