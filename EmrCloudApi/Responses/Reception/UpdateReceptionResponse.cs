namespace EmrCloudApi.Responses.Reception;

public class UpdateReceptionResponse
{
    public UpdateReceptionResponse(bool success)
    {
        Success = success;
    }

    public bool Success { get; private set; }
}
