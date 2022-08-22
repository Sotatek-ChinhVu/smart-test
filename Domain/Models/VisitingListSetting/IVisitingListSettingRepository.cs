using Domain.Models.SystemConf;
using Domain.Models.UserConfig;

namespace Domain.Models.VisitingListSetting;

public interface IVisitingListSettingRepository
{
    void Save(List<UserConfigModel> userConfModels, List<SystemConfModel> systemConfModels);
}
