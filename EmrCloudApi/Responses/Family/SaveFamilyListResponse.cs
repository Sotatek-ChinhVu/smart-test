namespace EmrCloudApi.Responses.Family;

public class SaveFamilyListResponse
{
    public SaveFamilyListResponse(bool success)
    {
        Success = success;
    }

    public bool Success { get; private set; }
}
