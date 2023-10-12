namespace EmrCloudApi.Responses.Logger;

public class WriteListLogResponse
{
    public WriteListLogResponse(bool success)
    {
        Success = success;
    }

    public bool Success { get;private set; }
}
