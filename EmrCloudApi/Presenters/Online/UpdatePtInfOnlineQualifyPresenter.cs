using EmrCloudApi.Constants;
using EmrCloudApi.Responses.Online;
using EmrCloudApi.Responses;
using UseCase.Online.UpdatePtInfOnlineQualify;

namespace EmrCloudApi.Presenters.Online;

public class UpdatePtInfOnlineQualifyPresenter : IUpdatePtInfOnlineQualifyOutputPort
{
    public Response<UpdatePtInfOnlineQualifyResponse> Result { get; private set; } = new();

    public void Complete(UpdatePtInfOnlineQualifyOutputData output)
    {
        Result.Data = new UpdatePtInfOnlineQualifyResponse(output.Status == UpdatePtInfOnlineQualifyStatus.Successed);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(UpdatePtInfOnlineQualifyStatus status) => status switch
    {
        UpdatePtInfOnlineQualifyStatus.Successed => ResponseMessage.Success,
        UpdatePtInfOnlineQualifyStatus.Failed => ResponseMessage.Failed,
        UpdatePtInfOnlineQualifyStatus.InvalidPtId => ResponseMessage.InvalidPtId,
        _ => string.Empty
    };
}

