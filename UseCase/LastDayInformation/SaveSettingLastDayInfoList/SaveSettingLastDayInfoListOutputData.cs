using UseCase.Core.Sync.Core;

namespace UseCase.LastDayInformation.SaveSettingLastDayInfoList;

public class SaveSettingLastDayInfoListOutputData : IOutputData
{
    public SaveSettingLastDayInfoListOutputData(SaveSettingLastDayInfoListStatus status)
    {
        Status = status;
    }

    public SaveSettingLastDayInfoListStatus Status { get; private set; }
}
