using UseCase.Core.Sync.Core;

namespace UseCase.VisitingList.SaveSettings;

public class SaveVisitingListSettingsOutputData : IOutputData
{
    public SaveVisitingListSettingsOutputData(SaveVisitingListSettingsStatus status)
    {
        Status = status;
    }

    public SaveVisitingListSettingsStatus Status { get; private set; }
}
