using Domain.Models.ColumnSetting;

namespace EmrCloudApi.Requests.ColumnSetting;

public class SaveColumnSettingListRequest
{
    public List<ColumnSettingModel> Settings { get; set; } = new();
}
