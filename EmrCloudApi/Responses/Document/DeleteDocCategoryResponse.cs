namespace EmrCloudApi.Responses.Document;

public class DeleteDocCategoryResponse
{
    public DeleteDocCategoryResponse(bool success)
    {
        Success = success;
    }

    public bool Success { get; private set; }
}
