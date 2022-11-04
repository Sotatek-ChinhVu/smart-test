using Domain.Models.VisitingListSetting;
using UseCase.Core.Sync.Core;

namespace UseCase.VisitingList.SaveSettings;

public class SaveVisitingListSettingsInputData : IInputData<SaveVisitingListSettingsOutputData>
{
    public SaveVisitingListSettingsInputData(VisitingListSettingModel settings, int hpId, int userId)
    {
        Settings = settings;
        HpId = hpId;
        UserId = userId;
    }

    public VisitingListSettingModel Settings { get; private set; }

    public int HpId { get; private set; }

    public int UserId { get; private set; }
}
