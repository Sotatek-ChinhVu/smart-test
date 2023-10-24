using Domain.Models.ColumnSetting;

namespace EmrCloudApi.Responses.ColumnSetting;

public class GetColumnSettingByTableNameListResponse
{
    public GetColumnSettingByTableNameListResponse(Dictionary<string, List<ColumnSettingModel>> settingList)
    {
        SettingList = settingList;
    }

    public Dictionary<string, List<ColumnSettingModel>> SettingList { get; private set; }
}
