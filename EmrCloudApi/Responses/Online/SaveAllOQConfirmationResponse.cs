namespace EmrCloudApi.Responses.Online;

public class SaveAllOQConfirmationResponse
{
    public SaveAllOQConfirmationResponse(bool successed)
    {
        Successed = successed;
    }

    public bool Successed { get; private set; }
}
