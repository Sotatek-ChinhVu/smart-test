namespace EmrCloudApi.Tenant.Responses.JsonSetting;

public class UpsertJsonSettingResponse
{
    public UpsertJsonSettingResponse(bool success)
    {
        Success = success;
    }

    public bool Success { get; private set; }
}
