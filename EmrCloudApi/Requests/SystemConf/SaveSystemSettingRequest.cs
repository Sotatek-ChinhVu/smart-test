using Domain.Models.HpInf;
using Domain.Models.MstItem;
using Domain.Models.Santei;
using Domain.Models.SystemConf;

namespace EmrCloudApi.Requests.SystemConf
{
    public class SaveSystemSettingRequest
    {
        public int HpId { get; set; }
        public int UserId { get; set; }
        public List<HpInfModel> HpInfs { get; set; } = new();
        public List<SystemConfMenuModel> SystemConfMenus { get; set; } = new();
        public List<SanteiInfDetailModel> SanteiInfs { get; set; } = new();
        public List<KensaCenterMstModel> KensaCenters { get; set; } = new();
    }
}
