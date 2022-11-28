namespace EmrCloudApi.Responses.Reception;

public class UpdateReceptionStaticCellResponse
{
    public UpdateReceptionStaticCellResponse(bool success)
    {
        Success = success;
    }

    public bool Success { get; private set; }
}
