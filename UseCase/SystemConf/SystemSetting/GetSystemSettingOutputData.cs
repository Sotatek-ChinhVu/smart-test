using Domain.Models.HpInf;
using Domain.Models.MstItem;
using Domain.Models.Santei;
using Domain.Models.SystemConf;
using UseCase.Core.Sync.Core;

namespace UseCase.SystemConf.SystemSetting
{
    public class GetSystemSettingOutputData : IOutputData
    {
        public GetSystemSettingOutputData(Dictionary<string, string> roudouMst, List<HpInfModel> hpInfs, List<SanteiInfDetailModel> santeiInfs, List<KensaCenterMstModel> kensaCenters, List<string> centerCds, List<SystemConfMenuModel> systemConfMenus, GetSystemSettingStatus status)
        {
            RoudouMst = roudouMst;
            HpInfs = hpInfs;
            SanteiInfs = santeiInfs;
            KensaCenters = kensaCenters;
            CenterCds = centerCds;
            SystemConfMenus = systemConfMenus;
            Status = status;
        }

        //Key: RoudouCd, Value: RoudouName
        public Dictionary<string, string> RoudouMst { get; private set; }
        public List<HpInfModel> HpInfs { get; private set; }
        public List<SanteiInfDetailModel> SanteiInfs { get; private set; }
        public List<KensaCenterMstModel> KensaCenters { get; private set; }
        public List<string> CenterCds { get; private set; }
        public List<SystemConfMenuModel> SystemConfMenus { get; private set; }
        public GetSystemSettingStatus Status { get; private set; }
    }
}
