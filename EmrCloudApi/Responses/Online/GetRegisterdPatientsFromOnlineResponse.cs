using EmrCloudApi.Responses.Online.Dto;

namespace EmrCloudApi.Responses.Online;

public class GetRegisterdPatientsFromOnlineResponse
{
    public GetRegisterdPatientsFromOnlineResponse(List<OnlineConfirmationHistoryDto> onlineConfirmationHistoryList)
    {
        OnlineConfirmationHistoryList = onlineConfirmationHistoryList;
    }

    public List<OnlineConfirmationHistoryDto> OnlineConfirmationHistoryList { get; private set; }
}
