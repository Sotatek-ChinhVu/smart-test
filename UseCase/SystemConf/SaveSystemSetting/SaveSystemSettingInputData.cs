using Domain.Models.HpInf;
using Domain.Models.MstItem;
using Domain.Models.Santei;
using Domain.Models.SystemConf;
using UseCase.Core.Sync.Core;

namespace UseCase.SystemConf.SaveSystemSetting
{
    public class SaveSystemSettingInputData : IInputData<SaveSystemSettingOutputData>
    {
        public SaveSystemSettingInputData(int hpId, int userId, List<HpInfModel> hpInfs, List<SystemConfMenuModel> systemConfMenus, List<SanteiInfDetailModel> santeiInfs, List<KensaCenterMstModel> kensaCenters)
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
        public List<HpInfModel> HpInfs { get; private set; }
        public List<SystemConfMenuModel> SystemConfMenus { get; private set; }
        public List<SanteiInfDetailModel> SanteiInfs { get; private set; }
        public List<KensaCenterMstModel> KensaCenters { get; private set; }
    }
}
