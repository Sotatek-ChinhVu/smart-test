using Domain.Models.HpInf;
using Domain.Models.SystemConf;
using UseCase.Core.Sync.Core;

namespace UseCase.SystemConf.SaveSystemSetting
{
    public class SaveSystemSettingInputData : IInputData<SaveSystemSettingOutputData>
    {
        public SaveSystemSettingInputData(int hpId, int userId, List<HpInfModel> hpInfs, List<SystemConfMenuModel> systemConfMenus, bool isUpdateHpInfo, bool isUpdateSystemGenerationConf)
        {
            HpId = hpId;
            UserId = userId;
            HpInfs = hpInfs;
            SystemConfMenus = systemConfMenus;
            IsUpdateHpInfo = isUpdateHpInfo;
            IsUpdateSystemGenerationConf = isUpdateSystemGenerationConf;
        }

        public int HpId { get; private set; }
        public int UserId { get; private set; }
        public List<HpInfModel> HpInfs { get; private set; }
        public List<SystemConfMenuModel> SystemConfMenus { get; private set; }
        public bool IsUpdateHpInfo { get; private set; }
        public bool IsUpdateSystemGenerationConf { get; private set; }
    }
}
