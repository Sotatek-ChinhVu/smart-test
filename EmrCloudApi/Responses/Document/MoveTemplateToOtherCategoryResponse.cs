namespace EmrCloudApi.Responses.Document;

public class MoveTemplateToOtherCategoryResponse
{
    public MoveTemplateToOtherCategoryResponse(bool success)
    {
        Success = success;
    }

    public bool Success { get; private set; }
}
