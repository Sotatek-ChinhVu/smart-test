namespace EmrCloudApi.Responses.Document;

public class AddTemplateToCategoryResponse
{
    public AddTemplateToCategoryResponse(bool success)
    {
        Success = success;
    }

    public bool Success { get; private set; }
}
