namespace EmrCloudApi.Tenant.Responses.Reception;

public class InsertReceptionResponse
{
    public InsertReceptionResponse(bool success)
    {
        Success = success;
    }

    public bool Success { get; private set; }
}
