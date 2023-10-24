namespace EmrCloudApi.Requests.ColumnSetting;

public class GetColumnSettingByTableNameListRequest
{
    public List<string> TableNameList { get; set; } = new();
}
