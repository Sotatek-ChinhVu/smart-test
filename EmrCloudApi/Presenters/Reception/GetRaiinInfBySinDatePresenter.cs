using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Reception;
using UseCase.Reception.GetRaiinInfBySinDate;

namespace EmrCloudApi.Presenters.Reception;

public class GetRaiinInfBySinDatePresenter : IGetRaiinInfBySinDateOutputPort
{
    public Response<GetRaiinInfBySinDateResponse> Result { get; private set; } = new();

    public void Complete(GetRaiinInfBySinDateOutputData output)
    {
        Result.Data = new GetRaiinInfBySinDateResponse(new RaiinInfDto(output.RaiinInf));
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetRaiinInfBySinDateStatus status) => status switch
    {
        GetRaiinInfBySinDateStatus.Successed => ResponseMessage.Success,
        _ => string.Empty

    };
}
