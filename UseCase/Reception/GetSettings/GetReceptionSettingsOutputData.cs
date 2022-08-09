using Domain.Models.Reception;
using Domain.Models.VisitingListSetting;
using UseCase.Core.Sync.Core;

namespace UseCase.Reception.GetSettings;

public class GetReceptionSettingsOutputData : IOutputData
{
    public GetReceptionSettingsOutputData(GetReceptionSettingsStatus status, VisitingListSettingModel settings)
    {
        Status = status;
        Settings = settings;
    }

    public GetReceptionSettingsStatus Status { get; private set; }
    public VisitingListSettingModel Settings { get; private set; }
}
