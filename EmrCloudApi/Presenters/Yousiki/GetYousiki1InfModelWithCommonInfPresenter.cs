using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Yousiki;
using EmrCloudApi.Responses.Yousiki.Dto;
using UseCase.Yousiki.GetYousiki1InfModelWithCommonInf;

namespace EmrCloudApi.Presenters.Yousiki;

public class GetYousiki1InfModelWithCommonInfPresenter : IGetYousiki1InfModelWithCommonInfOutputPort
{
    public Response<GetYousiki1InfModelWithCommonInfResponse> Result { get; private set; } = new();

    public void Complete(GetYousiki1InfModelWithCommonInfOutputData output)
    {
        Result.Data = new GetYousiki1InfModelWithCommonInfResponse(output.Yousiki1InfList.Select(item => new Yousiki1InfDto(item)).ToList());
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetYousiki1InfModelWithCommonInfStatus status) => status switch
    {
        GetYousiki1InfModelWithCommonInfStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}
