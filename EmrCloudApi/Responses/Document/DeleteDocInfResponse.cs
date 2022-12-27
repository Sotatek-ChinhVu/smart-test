namespace EmrCloudApi.Responses.Document;

public class DeleteDocInfResponse
{
    public DeleteDocInfResponse(bool success)
    {
        Success = success;
    }

    public bool Success { get; private set; }
}
