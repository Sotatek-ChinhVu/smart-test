using EmrCloudApi.Responses.Online.Dto;

namespace EmrCloudApi.Responses.Online;

public class GetOnlineConsentResponse
{
    public GetOnlineConsentResponse(List<OnlineConsentDto> onlineConsentList)
    {
        OnlineConsentList = onlineConsentList;
    }

    public List<OnlineConsentDto> OnlineConsentList { get; private set; }
}
