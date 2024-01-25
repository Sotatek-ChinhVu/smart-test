using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Yousiki;
using EmrCloudApi.Responses.Yousiki.Dto;
using UseCase.Yousiki.GetYousiki1InfDetails;

namespace EmrCloudApi.Presenters.Yousiki;

public class GetYousiki1InfDetailsPresenter : IGetYousiki1InfDetailsOutputPort
{
    public Response<GetYousiki1InfDetailsResponse> Result { get; private set; } = new();

    public void Complete(GetYousiki1InfDetailsOutputData output)
    {
        Result.Data = new GetYousiki1InfDetailsResponse(new Yousiki1InfDto(output.Yousiki1Inf));
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetYousiki1InfDetailsStatus status) => status switch
    {
        GetYousiki1InfDetailsStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}
