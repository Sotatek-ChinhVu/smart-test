namespace EmrCloudApi.Responses.Online;

public class UpdatePtInfOnlineQualifyResponse
{
    public UpdatePtInfOnlineQualifyResponse(bool successed)
    {
        Successed = successed;
    }

    public bool Successed { get; private set; }
}
