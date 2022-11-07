namespace EmrCloudApi.Tenant.Requests.ColumnSetting;

public class GetColumnSettingListRequest
{
    public int UserId { get; set; }
    public string TableName { get; set; } = string.Empty;
}
