using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Online;
using UseCase.Online.UpdateOnlineInRaiinInf;

namespace EmrCloudApi.Presenters.Online;

public class UpdateOnlineInRaiinInfPresenter : IUpdateOnlineInRaiinInfOutputPort
{
    public Response<UpdateOnlineInRaiinInfResponse> Result { get; private set; } = new();

    public void Complete(UpdateOnlineInRaiinInfOutputData output)
    {
        Result.Data = new UpdateOnlineInRaiinInfResponse(output.Status == UpdateOnlineInRaiinInfStatus.Successed);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(UpdateOnlineInRaiinInfStatus status) => status switch
    {
        UpdateOnlineInRaiinInfStatus.Successed => ResponseMessage.Success,
        UpdateOnlineInRaiinInfStatus.Failed => ResponseMessage.Failed,
        UpdateOnlineInRaiinInfStatus.InvalidPtId => ResponseMessage.InvalidPtId,
        _ => string.Empty
    };
}

