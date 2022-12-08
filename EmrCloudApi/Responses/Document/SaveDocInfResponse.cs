namespace EmrCloudApi.Responses.Document;

public class SaveDocInfResponse
{
    public SaveDocInfResponse(bool success)
    {
        Success = success;
    }

    public bool Success { get; private set; }
}
