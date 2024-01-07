namespace EmrCloudApi.Responses.Diseases;

public class IsHokenInfInUsedResponse
{
    public IsHokenInfInUsedResponse(bool result)
    {
        Result = result;
    }

    public bool Result { get; private set; }
}
