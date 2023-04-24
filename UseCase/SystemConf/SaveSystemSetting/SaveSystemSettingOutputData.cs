using UseCase.Core.Sync.Core;

namespace UseCase.SystemConf.SaveSystemSetting
{
    public class SaveSystemSettingOutputData : IOutputData
    {
        public SaveSystemSettingOutputData(SaveSystemSettingStatus status)
        {
            Status = status;
        }

        public SaveSystemSettingStatus Status { get; private set; }
    }
}
