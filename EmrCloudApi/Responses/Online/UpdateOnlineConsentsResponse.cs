namespace EmrCloudApi.Responses.Online;

public class UpdateOnlineConsentsResponse
{
    public UpdateOnlineConsentsResponse(bool successed)
    {
        Successed = successed;
    }

    public bool Successed { get; private set; }
}
