using Domain.Models.HpInf;
using Domain.Models.MstItem;
using Domain.Models.Santei;
using Domain.Models.SystemConf;

namespace EmrCloudApi.Responses.SystemConf
{
    public class GetSystemSettingResponse
    {
        public GetSystemSettingResponse(Dictionary<string, string> roudouMst, List<HpInfModel> hpInfs, List<SanteiInfDetailModel> santeiInfs, List<KensaCenterMstModel> kensaCenters, List<string> centerCds, List<SystemConfMenuModel> systemConfMenus)
        {
            RoudouMst = roudouMst;
            HpInfs = hpInfs;
            SanteiInfs = santeiInfs;
            KensaCenters = kensaCenters;
            CenterCds = centerCds;
            SystemConfMenus = systemConfMenus;
        }

        public Dictionary<string, string> RoudouMst { get; private set; }
        public List<HpInfModel> HpInfs { get; private set; }
        public List<SanteiInfDetailModel> SanteiInfs { get; private set; }
        public List<KensaCenterMstModel> KensaCenters { get; private set; }
        public List<string> CenterCds { get; private set; }
        public List<SystemConfMenuModel> SystemConfMenus { get; private set; }
    }
}
