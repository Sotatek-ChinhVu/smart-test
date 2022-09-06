namespace EmrCloudApi.Tenant.Responses.Schema;

public class SaveImageResponse
{
    public SaveImageResponse(string data)
    {
        Data = data;
    }

    public string Data { get; private set; }
}
