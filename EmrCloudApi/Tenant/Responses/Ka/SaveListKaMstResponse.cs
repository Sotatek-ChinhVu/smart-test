namespace EmrCloudApi.Tenant.Responses.Ka;

public class SaveListKaMstResponse
{
    public SaveListKaMstResponse(bool status)
    {
        Status = status;
    }

    public bool Status { get; private set; }
}
