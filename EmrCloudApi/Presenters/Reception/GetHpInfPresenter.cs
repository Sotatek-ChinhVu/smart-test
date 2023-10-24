using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Reception;
using UseCase.Reception.GetHpInf;

namespace EmrCloudApi.Presenters.Reception;

public class GetHpInfPresenter : IGetHpInfOutputPort
{
    public Response<GetHpInfResponse> Result { get; private set; } = new();

    public void Complete(GetHpInfOutputData output)
    {
        Result.Data = new GetHpInfResponse(new HpInfDto(output.HpInf));
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetHpInfStatus status) => status switch
    {
        GetHpInfStatus.Successed => ResponseMessage.Success,
        _ => string.Empty

    };
}
