using Domain.Models.ColumnSetting;
using UseCase.Core.Sync.Core;

namespace UseCase.ColumnSetting.SaveList;

public class SaveColumnSettingListInputData : IInputData<SaveColumnSettingListOutputData>
{
    public SaveColumnSettingListInputData(List<ColumnSettingModel> settings)
    {
        Settings = settings;
    }

    public List<ColumnSettingModel> Settings { get; private set; }
}
