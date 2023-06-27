namespace EmrCloudApi.Requests.ColumnSetting;

public class SaveColumnSettingListRequest
{
    public List<ColumnSettingRequestItem> Settings { get; set; } = new();
}
