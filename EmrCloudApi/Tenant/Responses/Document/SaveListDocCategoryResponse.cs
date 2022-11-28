namespace EmrCloudApi.Tenant.Responses.Document;

public class SaveListDocCategoryResponse
{
    public SaveListDocCategoryResponse(bool success)
    {
        Success = success;
    }

    public bool Success { get; private set; }
}
