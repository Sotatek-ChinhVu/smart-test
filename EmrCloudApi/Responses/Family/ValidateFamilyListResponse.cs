namespace EmrCloudApi.Responses.Family;

public class ValidateFamilyListResponse
{
    public ValidateFamilyListResponse(bool success)
    {
        Success = success;
    }

    public bool Success { get; private set; }
}
