namespace EmrCloudApi.Responses.Yousiki;

public class DeleteYousikiInfResponse
{
    public DeleteYousikiInfResponse(bool successed)
    {
        Successed = successed;
    }

    public bool Successed { get; private set; }
}
