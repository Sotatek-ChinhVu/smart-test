namespace EmrCloudApi.Responses.Schema;

public class SaveListFileTodayOrderResponse
{
    public SaveListFileTodayOrderResponse(bool success)
    {
        Success = success;
    }

    public bool Success { get; private set; }
}
