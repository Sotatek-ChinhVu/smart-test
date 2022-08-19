using Domain.Models.ColumnSetting;
using UseCase.Core.Sync.Core;

namespace UseCase.ColumnSetting.GetList;

public class GetColumnSettingListOutputData : IOutputData
{
    public GetColumnSettingListOutputData(GetColumnSettingListStatus status, List<ColumnSettingModel> settings)
    {
        Status = status;
        Settings = settings;
    }

    public GetColumnSettingListStatus Status { get; private set; }
    public List<ColumnSettingModel> Settings { get; private set; }
}
