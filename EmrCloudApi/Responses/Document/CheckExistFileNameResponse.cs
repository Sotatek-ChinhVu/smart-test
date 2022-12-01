namespace EmrCloudApi.Responses.Document;

public class CheckExistFileNameResponse
{
    public CheckExistFileNameResponse(bool success)
    {
        Success = success;
    }

    public bool Success { get; private set; }
}
