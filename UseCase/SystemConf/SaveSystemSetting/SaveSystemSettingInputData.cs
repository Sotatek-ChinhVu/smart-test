using UseCase.Core.Sync.Core;

namespace UseCase.SystemConf.SaveSystemSetting
{
    public class SaveSystemSettingInputData : IInputData<SaveSystemSettingOutputData>
    {
        public SaveSystemSettingInputData(int hpId, int userId, List<HpInfItem> hpInfs, List<SystemConfMenuItem> systemConfMenus, List<SanteiInfDetailItem> santeiInfs, List<KensaCenterMstItem> kensaCenters)
        {
            HpId = hpId;
            UserId = userId;
            HpInfs = hpInfs;
            SystemConfMenus = systemConfMenus;
            SanteiInfs = santeiInfs;
            KensaCenters = kensaCenters;
        }

        public int HpId { get; private set; }
        public int UserId { get; private set; }
        public List<HpInfItem> HpInfs { get; private set; }
        public List<SystemConfMenuItem> SystemConfMenus { get; private set; }
        public List<SanteiInfDetailItem> SanteiInfs { get; private set; }
        public List<KensaCenterMstItem> KensaCenters { get; private set; }
    }
}
