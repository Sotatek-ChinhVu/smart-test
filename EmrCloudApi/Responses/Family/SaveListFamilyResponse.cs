namespace EmrCloudApi.Responses.Family;

public class SaveListFamilyResponse
{
    public SaveListFamilyResponse(bool success)
    {
        Success = success;
    }

    public bool Success { get; private set; }
}
