namespace EmrCloudApi.Responses.MainMenu;

public class DeleteKensaInfResponse
{
    public DeleteKensaInfResponse(bool successed)
    {
        Successed = successed;
    }

    public bool Successed { get;private set; }
}
