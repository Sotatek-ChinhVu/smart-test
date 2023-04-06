namespace EmrCloudApi.Responses.Receipt;

public class SaveReceStatusResponse
{
    public SaveReceStatusResponse(bool success)
    {
        Success = success;
    }

    public bool Success { get; private set; }
}
