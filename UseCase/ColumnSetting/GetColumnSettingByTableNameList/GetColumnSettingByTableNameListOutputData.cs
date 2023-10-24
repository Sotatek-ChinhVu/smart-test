using Domain.Models.ColumnSetting;
using UseCase.Core.Sync.Core;

namespace UseCase.ColumnSetting.GetColumnSettingByTableNameList;

public class GetColumnSettingByTableNameListOutputData : IOutputData
{
    public GetColumnSettingByTableNameListOutputData(Dictionary<string, List<ColumnSettingModel>> settingList, GetColumnSettingByTableNameListStatus status)
    {
        SettingList = settingList;
        Status = status;
    }

    public Dictionary<string, List<ColumnSettingModel>> SettingList { get; private set; }

    public GetColumnSettingByTableNameListStatus Status { get; private set; }
}
