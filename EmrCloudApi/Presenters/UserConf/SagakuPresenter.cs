using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.UserConf;
using UseCase.User.Sagaku;

namespace EmrCloudApi.Presenters.UserConf;

public class SagakuPresenter : ISagakuOutputPort
{
    public Response<SagakuResponse> Result { get; private set; } = new Response<SagakuResponse>();

    public void Complete(SagakuOutputData output)
    {
        Result.Data = new SagakuResponse(output.Value);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(SagakuStatus status) => status switch
    {
        SagakuStatus.Successed => ResponseMessage.Success,
        SagakuStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        SagakuStatus.InvalidUserId => ResponseMessage.InvalidUserId,
        SagakuStatus.Failed => ResponseMessage.Failed,
        _ => string.Empty
    };
}
