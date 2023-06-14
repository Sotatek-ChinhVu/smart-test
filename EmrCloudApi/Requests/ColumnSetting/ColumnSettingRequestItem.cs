namespace EmrCloudApi.Requests.ColumnSetting;

public class ColumnSettingRequestItem
{
    public string TableName { get; set; } = string.Empty;

    public string ColumnName { get; set; } = string.Empty;

    public int DisplayOrder { get; set; }

    public bool IsPinned { get; set; }

    public bool IsHidden { get; set; }

    public int Width { get; set; }

    public string OrderBy { get; set; } = string.Empty;
}
