using Domain.Models.SystemConf;

namespace EmrCloudApi.Responses.SystemConf
{
    public class GetSystemSettingResponse
    {
        public GetSystemSettingResponse(List<SystemConfMenuModel> systemConfMenus)
        {
            SystemConfMenus = systemConfMenus;
        }

        public List<SystemConfMenuModel> SystemConfMenus { get; private set; }
    }
}
