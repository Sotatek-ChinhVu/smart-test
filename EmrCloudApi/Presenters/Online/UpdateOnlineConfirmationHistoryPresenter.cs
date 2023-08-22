using EmrCloudApi.Constants;
using EmrCloudApi.Responses.Online;
using EmrCloudApi.Responses;
using UseCase.Online.UpdateOnlineConfirmationHistory;

namespace EmrCloudApi.Presenters.Online;

public class UpdateOnlineConfirmationHistoryPresenter : IUpdateOnlineConfirmationHistoryOutputPort
{
    public Response<UpdateOnlineConfirmationHistoryResponse> Result { get; private set; } = new();

    public void Complete(UpdateOnlineConfirmationHistoryOutputData output)
    {
        Result.Data = new UpdateOnlineConfirmationHistoryResponse(output.UpdateSuccessed);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(UpdateOnlineConfirmationHistoryStatus status) => status switch
    {
        UpdateOnlineConfirmationHistoryStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}