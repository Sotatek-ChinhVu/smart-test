namespace EmrCloudApi.Responses.SystemConf;

public class SavePathResponse
{
    public SavePathResponse(bool success)
    {
        Success = success;
    }

    public bool Success { get; private set; }
}
