using UseCase.Core.Sync.Core;

namespace UseCase.RaiinListSetting.SaveRaiinListSetting
{
    public class SaveRaiinListSettingOutputData : IOutputData
    {
        public SaveRaiinListSettingOutputData(SaveRaiinListSettingStatus status)
        {
            Status = status;
        }

        public SaveRaiinListSettingStatus Status { get; private set; }
    }
}
