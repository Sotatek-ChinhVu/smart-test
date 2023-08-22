using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Reception;
using UseCase.Reception.GetYoyakuRaiinInf;

namespace EmrCloudApi.Presenters.Reception;

public class GetYoyakuRaiinInfPresenter : IGetYoyakuRaiinInfOutputPort
{
    public Response<GetYoyakuRaiinInfResponse> Result { get; private set; } = new();

    public void Complete(GetYoyakuRaiinInfOutputData output)
    {
        Result.Data = new GetYoyakuRaiinInfResponse(new RaiinInfDto(output.RaiinInf));
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetYoyakuRaiinInfStatus status) => status switch
    {
        GetYoyakuRaiinInfStatus.Successed => ResponseMessage.Success,
        _ => string.Empty

    };
}
