using Domain.Common;

namespace Domain.Models.TimeZoneConf
{
    public interface ITimeZoneConfRepository : IRepositoryBase
    {
        List<TimeZoneConfGroupModel> GetTimeZoneConfGroupModels(int hpId);
    }
}
