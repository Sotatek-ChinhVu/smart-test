using Domain.Models.VisitingListSetting;
using UseCase.Core.Sync.Core;

namespace UseCase.VisitingList.SaveSettings;

public class SaveVisitingListSettingsInputData : IInputData<SaveVisitingListSettingsOutputData>
{
    public SaveVisitingListSettingsInputData(int userId, VisitingListSettingModel settings)
    {
        UserId = userId;
        Settings = settings;
    }

    public int UserId { get; private set; }
    public VisitingListSettingModel Settings { get; private set; }
}
