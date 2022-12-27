namespace EmrCloudApi.Responses.Document;

public class DeleteDocTemplateResponse
{
    public DeleteDocTemplateResponse(bool success)
    {
        Success = success;
    }

    public bool Success { get; private set; }
}
