using Domain.Models.ColumnSetting;
using UseCase.Core.Sync.Core;

namespace UseCase.Reception.GetVisitingColumnSettings;

public class GetVisitingColumnSettingsOutputData : IOutputData
{
    public GetVisitingColumnSettingsOutputData(GetVisitingColumnSettingsStatus status)
    {
        Status = status;
    }

    public GetVisitingColumnSettingsOutputData(GetVisitingColumnSettingsStatus status, List<ColumnSettingModel> settings)
    {
        Status = status;
        Settings = settings;
    }

    public GetVisitingColumnSettingsStatus Status { get; private set; }
    public List<ColumnSettingModel> Settings { get; private set; } = new();
}
