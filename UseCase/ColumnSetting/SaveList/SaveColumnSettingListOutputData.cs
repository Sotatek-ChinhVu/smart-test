using UseCase.Core.Sync.Core;

namespace UseCase.ColumnSetting.SaveList;

public class SaveColumnSettingListOutputData : IOutputData
{
    public SaveColumnSettingListOutputData(SaveColumnSettingListStatus status)
    {
        Status = status;
    }

    public SaveColumnSettingListStatus Status { get; private set; }
}
