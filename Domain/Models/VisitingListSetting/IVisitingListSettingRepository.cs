using Domain.Common;
using Domain.Models.SystemConf;

namespace Domain.Models.VisitingListSetting;

public interface IVisitingListSettingRepository : IRepositoryBase
{
    void Save(List<SystemConfModel> systemConfModels, int hpId, int userId);
}
