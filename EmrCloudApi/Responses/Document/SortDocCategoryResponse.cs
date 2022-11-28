namespace EmrCloudApi.Responses.Document;

public class SortDocCategoryResponse
{
    public SortDocCategoryResponse(bool success)
    {
        Success = success;
    }

    public bool Success { get; private set; }
}
