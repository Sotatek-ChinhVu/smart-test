namespace EmrCloudApi.Responses.Yousiki;

public class SaveDetailDefaultResponse
{
    public SaveDetailDefaultResponse(bool successd)
    {
        Successd = successd;
    }

    public bool Successd { get; private set; }
}
