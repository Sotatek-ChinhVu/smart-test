namespace EmrCloudApi.Responses.Online;

public class UpdateOQConfirmationResponse
{
    public UpdateOQConfirmationResponse(bool successed)
    {
        Successed = successed;
    }

    public bool Successed { get; private set; }
}
