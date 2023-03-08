namespace EmrCloudApi.Responses.Receipt;

public class SaveReceCheckOptResponse
{
    public SaveReceCheckOptResponse(bool success)
    {
        Success = success;
    }

    public bool Success { get; private set; }
}
