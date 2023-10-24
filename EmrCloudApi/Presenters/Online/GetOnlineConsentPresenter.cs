using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Online;
using EmrCloudApi.Responses.Online.Dto;
using UseCase.Online.GetOnlineConsent;

namespace EmrCloudApi.Presenters.Online;

public class GetOnlineConsentPresenter : IGetOnlineConsentOutputPort
{
    public Response<GetOnlineConsentResponse> Result { get; private set; } = new();

    public void Complete(GetOnlineConsentOutputData output)
    {
        Result.Data = new GetOnlineConsentResponse(output.OnlineConsentList.Select(item => new OnlineConsentDto(item)).ToList());
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetOnlineConsentStatus status) => status switch
    {
        GetOnlineConsentStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}
