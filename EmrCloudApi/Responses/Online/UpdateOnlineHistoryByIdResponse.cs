namespace EmrCloudApi.Responses.Online;

public class UpdateOnlineHistoryByIdResponse
{
    public UpdateOnlineHistoryByIdResponse(bool updateSuccess)
    {
        UpdateSuccess = updateSuccess;
    }

    public bool UpdateSuccess { get; private set; }
}
