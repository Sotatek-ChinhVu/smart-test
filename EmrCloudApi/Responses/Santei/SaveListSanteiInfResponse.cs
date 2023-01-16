namespace EmrCloudApi.Responses.Santei;

public class SaveListSanteiInfResponse
{
    public SaveListSanteiInfResponse(bool success)
    {
        Success = success;
    }

    public bool Success { get; private set; }
}
