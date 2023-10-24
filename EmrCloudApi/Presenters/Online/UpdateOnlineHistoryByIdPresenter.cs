using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Online;
using UseCase.Online.UpdateOnlineHistoryById;

namespace EmrCloudApi.Presenters.Online;

public class UpdateOnlineHistoryByIdPresenter : IUpdateOnlineHistoryByIdOutputPort
{
    public Response<UpdateOnlineHistoryByIdResponse> Result { get; private set; } = new();

    public void Complete(UpdateOnlineHistoryByIdOutputData output)
    {
        Result.Data = new UpdateOnlineHistoryByIdResponse(output.Status == UpdateOnlineHistoryByIdStatus.Successed);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(UpdateOnlineHistoryByIdStatus status) => status switch
    {
        UpdateOnlineHistoryByIdStatus.Successed => ResponseMessage.Success,
        UpdateOnlineHistoryByIdStatus.Failed => ResponseMessage.Failed,
        UpdateOnlineHistoryByIdStatus.InvalidId => ResponseMessage.InvalidOnlineId,
        UpdateOnlineHistoryByIdStatus.InvalidPtId => ResponseMessage.InvalidPtId,
        UpdateOnlineHistoryByIdStatus.InvalidUketukeStatus => ResponseMessage.InvalidUketukeStatus,
        _ => string.Empty
    };
}
