using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Online;
using UseCase.Online.SaveAllOQConfirmation;

namespace EmrCloudApi.Presenters.Online;

public class SaveAllOQConfirmationPresenter : ISaveAllOQConfirmationOutputPort
{
    public Response<SaveAllOQConfirmationResponse> Result { get; private set; } = new();

    public void Complete(SaveAllOQConfirmationOutputData output)
    {
        Result.Data = new SaveAllOQConfirmationResponse(output.Status == SaveAllOQConfirmationStatus.Successed);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(SaveAllOQConfirmationStatus status) => status switch
    {
        SaveAllOQConfirmationStatus.Successed => ResponseMessage.Success,
        SaveAllOQConfirmationStatus.Failed => ResponseMessage.Failed,
        SaveAllOQConfirmationStatus.InvalidPtId => ResponseMessage.InvalidPtId,
        _ => string.Empty
    };
}
