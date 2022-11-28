using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Reception;
using UseCase.Reception.GetLastRaiinInfs;

namespace EmrCloudApi.Presenters.Reception;

public class GetLastRaiinInfsPresenter
{
    public Response<GetLastRaiinInfsResponse> Result { get; private set; } = new();

    public void Complete(GetLastRaiinInfsOutputData output)
    {
        Result.Data = new GetLastRaiinInfsResponse(output.ListReceptions);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetLastRaiinInfsStatus status) => status switch
    {
        GetLastRaiinInfsStatus.Successed => ResponseMessage.Success,
        GetLastRaiinInfsStatus.Failed => ResponseMessage.Failed,
        GetLastRaiinInfsStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        GetLastRaiinInfsStatus.InvalidPtId => ResponseMessage.InvalidPtId,
        GetLastRaiinInfsStatus.InvalidSinDate => ResponseMessage.InvalidSinDate,
        _ => string.Empty
    };
}
