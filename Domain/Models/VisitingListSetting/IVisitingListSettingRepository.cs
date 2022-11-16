using Domain.Models.SystemConf;

namespace Domain.Models.VisitingListSetting;

public interface IVisitingListSettingRepository
{
    void Save(List<SystemConfModel> systemConfModels, int hpId, int userId);
}
