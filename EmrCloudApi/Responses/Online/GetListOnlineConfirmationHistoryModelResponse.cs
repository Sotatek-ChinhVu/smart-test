using EmrCloudApi.Responses.Online.Dto;

namespace EmrCloudApi.Responses.Online;

public class GetListOnlineConfirmationHistoryModelResponse
{
    public GetListOnlineConfirmationHistoryModelResponse(List<OnlineConfirmationHistoryDto> onlineConfirmationHistoryList)
    {
        OnlineConfirmationHistoryList = onlineConfirmationHistoryList;
    }

    public List<OnlineConfirmationHistoryDto> OnlineConfirmationHistoryList { get; private set; }
}
