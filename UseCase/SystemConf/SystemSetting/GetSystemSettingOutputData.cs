using Domain.Models.SystemConf;
using UseCase.Core.Sync.Core;

namespace UseCase.SystemConf.SystemSetting
{
    public class GetSystemSettingOutputData : IOutputData
    {
        public GetSystemSettingOutputData(List<SystemConfMenuModel> systemConfMenus, GetSystemSettingStatus status)
        {
            SystemConfMenus = systemConfMenus;
            Status = status;
        }

        public List<SystemConfMenuModel> SystemConfMenus { get; private set; }
        public GetSystemSettingStatus Status { get; private set; }
    }
}
