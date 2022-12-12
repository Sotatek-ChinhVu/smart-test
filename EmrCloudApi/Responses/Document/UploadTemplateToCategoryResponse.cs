namespace EmrCloudApi.Responses.Document;

public class UploadTemplateToCategoryResponse
{
    public UploadTemplateToCategoryResponse(bool success)
    {
        Success = success;
    }

    public bool Success { get; private set; }
}
