namespace EmrCloudApi.Responses.Online;

public class SaveOQConfirmationResponse
{
    public SaveOQConfirmationResponse(bool successed)
    {
        Successed = successed;
    }

    public bool Successed { get; private set; }
}
