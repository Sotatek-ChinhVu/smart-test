namespace EmrCloudApi.Responses.Document;

public class ReplaceParamTemplateResponse
{
    public ReplaceParamTemplateResponse(bool success)
    {
        Success = success;
    }

    public bool Success { get; private set; }
}
