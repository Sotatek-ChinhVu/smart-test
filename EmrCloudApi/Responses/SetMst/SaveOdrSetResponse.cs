namespace EmrCloudApi.Responses.SetMst;

public class SaveOdrSetResponse
{
    public SaveOdrSetResponse(bool successed)
    {
        Successed = successed;
    }

    public bool Successed { get; private set; }
}
