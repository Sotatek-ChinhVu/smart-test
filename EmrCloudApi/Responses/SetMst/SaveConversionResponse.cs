namespace EmrCloudApi.Responses.SetMst;

public class SaveConversionResponse
{
    public SaveConversionResponse(bool successed)
    {
        Successed = successed;
    }

    public bool Successed { get; private set; }
}
