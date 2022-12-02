namespace EmrCloudApi.Responses.Reception;

public class UpdateReceptionDynamicCellResponse
{
    public UpdateReceptionDynamicCellResponse(bool success)
    {
        Success = success;
    }

    public bool Success { get; private set; }
}
