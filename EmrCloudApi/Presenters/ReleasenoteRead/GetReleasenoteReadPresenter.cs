using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.ReleasenoteRead;
using UseCase.Releasenote.ReleasenoteRead;
using UseCase.ReleasenoteRead;

namespace EmrCloudApi.Presenters.ReleasenoteRead;

public class GetReleasenoteReadPresenter : IGetListReleasenoteReadOutputPort
{
    public Response<GetReleasenoteReadResponse> Result { get; private set; } = new();

    public void Complete(GetListReleasenoteReadOutputData outputData)
    {
        Result.Data = new GetReleasenoteReadResponse(outputData.Version);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(GetListReleasenoteReadStatus status) => status switch
    {
        GetListReleasenoteReadStatus.Successed => ResponseMessage.Success,
        GetListReleasenoteReadStatus.Failed => ResponseMessage.Failed,
        _ => string.Empty
    };
}
