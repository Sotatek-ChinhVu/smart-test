using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Online;
using UseCase.Online.UpdateRefNo;

namespace EmrCloudApi.Presenters.Online;

public class UpdateRefNoPresenter : IUpdateRefNoOutputPort
{
    public Response<UpdateRefNoResponse> Result { get; private set; } = new();

    public void Complete(UpdateRefNoOutputData output)
    {
        Result.Data = new UpdateRefNoResponse(output.NextRefNo);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(UpdateRefNoStatus status) => status switch
    {
        UpdateRefNoStatus.Successed => ResponseMessage.Success,
        UpdateRefNoStatus.Failed => ResponseMessage.Failed,
        UpdateRefNoStatus.InvalidPtId => ResponseMessage.InvalidPtId,
        _ => string.Empty
    };
}
