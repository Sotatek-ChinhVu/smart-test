using Domain.Models.HpInf;
using Domain.Models.Santei;
using Domain.Models.SystemConf;

namespace EmrCloudApi.Responses.SystemConf
{
    public class GetSystemSettingResponse
    {
        public GetSystemSettingResponse(Dictionary<string, string> roudouMst, List<HpInfModel> hpInfs, List<SanteiInfDetailModel> santeiInfs, List<SystemConfMenuModel> systemConfMenus)
        {
            RoudouMst = roudouMst;
            HpInfs = hpInfs;
            SystemConfMenus = systemConfMenus;
            SanteiInfs = santeiInfs;
        }

        public Dictionary<string, string> RoudouMst { get; private set; }
        public List<HpInfModel> HpInfs { get; private set; }
        public List<SanteiInfDetailModel> SanteiInfs { get; private set; }
        public List<SystemConfMenuModel> SystemConfMenus { get; private set; }
    }
}
