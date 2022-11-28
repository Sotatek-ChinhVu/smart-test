namespace EmrCloudApi.Responses.ColumnSetting;

public class SaveColumnSettingListResponse
{
    public SaveColumnSettingListResponse(bool success)
    {
        Success = success;
    }

    public bool Success { get; private set; }
}
