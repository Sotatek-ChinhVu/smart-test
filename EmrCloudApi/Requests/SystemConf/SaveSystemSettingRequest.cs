using Domain.Models.HpInf;
using Domain.Models.SystemConf;

namespace EmrCloudApi.Requests.SystemConf
{
    public class SaveSystemSettingRequest
    {
        public int UserId { get; set; }
        public List<HpInfModel> HpInfs { get; set; } = new();
        public List<SystemConfMenuModel> SystemConfMenus { get; set; } = new();
        public bool IsUpdateHpInfo { get; set; }
        public bool IsUpdateSystemGenerationConf { get; set; }
    }
}
