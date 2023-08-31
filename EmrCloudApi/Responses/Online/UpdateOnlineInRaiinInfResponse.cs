namespace EmrCloudApi.Responses.Online;

public class UpdateOnlineInRaiinInfResponse
{
    public UpdateOnlineInRaiinInfResponse(bool successed)
    {
        Successed = successed;
    }

    public bool Successed { get; private set; }
}
