namespace EmrCloudApi.Responses.Online;

public class UpdateOnlineConfirmationHistoryResponse
{
    public UpdateOnlineConfirmationHistoryResponse(bool updateSuccessed)
    {
        UpdateSuccessed = updateSuccessed;
    }

    public bool UpdateSuccessed { get; private set; }
}
