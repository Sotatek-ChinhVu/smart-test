using Domain.Models.SystemConf;
using Domain.Models.SystemGenerationConf;

namespace UseCase.SystemConf.SystemSetting
{
    public class SystemConfMenuDto
    {
        public SystemConfMenuDto(SystemConfMenuModel systemConfMenu, List<SystemConfItemModel> systemConfItems, List<SystemGenerationConfModel> systemGenerationConfs)
        {
            SystemConfMenu = systemConfMenu;
            SystemConfItems = systemConfItems;
            SystemGenerationConfs = systemGenerationConfs;
        }

        public SystemConfMenuModel SystemConfMenu { get; private set; }
        public List<SystemConfItemModel> SystemConfItems { get; private set; }
        public List<SystemGenerationConfModel> SystemGenerationConfs { get; private set; }
    }
}
