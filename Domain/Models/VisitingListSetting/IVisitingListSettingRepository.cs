using Domain.Models.SystemConf;
using Domain.Models.UserConf;

namespace Domain.Models.VisitingListSetting;

public interface IVisitingListSettingRepository
{
    void Save(List<UserConfModel> userConfModels, List<SystemConfModel> systemConfModels);
}
