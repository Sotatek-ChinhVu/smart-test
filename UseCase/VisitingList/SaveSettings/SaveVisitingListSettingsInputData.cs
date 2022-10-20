using Domain.Models.VisitingListSetting;
using UseCase.Core.Sync.Core;

namespace UseCase.VisitingList.SaveSettings;

public class SaveVisitingListSettingsInputData : IInputData<SaveVisitingListSettingsOutputData>
{
    public SaveVisitingListSettingsInputData(VisitingListSettingModel settings)
    {
        Settings = settings;
    }

    public VisitingListSettingModel Settings { get; private set; }
}
