namespace EmrCloudApi.Responses.SystemConf;

public class SaveDrugCheckSettingResponse
{
    public SaveDrugCheckSettingResponse(bool success)
    {
        Success = success;
    }

    public bool Success { get; private set; }
}
